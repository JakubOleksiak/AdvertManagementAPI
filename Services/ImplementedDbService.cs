using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdvertApi.DTOS;
using AdvertApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AdvertApi.Services
{
    public class ImplementedDbService : IDbService
    {
        private readonly s18728Context _context;

        public ImplementedDbService(s18728Context context)
        {
            _context = context;
        }

        public Client RegisterClient(RegisterClientDTO dto)
        {
            var tmp = _context.Client.Where(e => e.Login.Equals(dto.Login)).ToList();
            if (tmp.Count>0)
            {
                return null;
            }

            Client client = new Client
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                Login = dto.Login,
            };
            int id = _context.Client.Max(e => e.IdClient) + 1;

            Password password = new Password
            {
                IdClient = id,
            };

            string saltHash = Salter.CreateSalt();
            string passHash = Salter.CreateHash(dto.Password, saltHash);
            password.PasswordHash = passHash;

            Salt salt = new Salt
            {
                IdClient = id,
                SaltHash = saltHash
            };

            _context.Client.Add(client);
            _context.Password.Add(password);
            _context.Salt.Add(salt);
            _context.SaveChanges();
            return client;
        }

        public Client GetClientFromRefreshToken(string refToken)
        {
            Client client;
            try
            {
                client = _context.Client.Single(e => e.RefreshToken.Equals(refToken));
            }
            catch (Exception e)
            {
                return null;
            }

            return client;

        }

        public string SetRefreshToken(string clientLogin, string refToken)
        {
            Client client;
            try
            {
                client = _context.Client.Single(e => e.Login.Equals(clientLogin));
            }
            catch (Exception e)
            {
                return null;
            }

            _context.Client.Update(client);
            client.RefreshToken = refToken;
            _context.SaveChanges();
            return refToken;
        }

        public List<Campaign> getCampaigns()
        {
            var campaigns = _context.Campaign.Include(e => e.Client).Include(e => e.Banners).ToList();
            return campaigns;
        }

        public bool checkBuildings(int dtoFromIdBuilding, int dtoToIdBuilding)
        {
            try
            {
                Building buildingFrom = _context.Building.Single(e=>e.IdBuilding==dtoFromIdBuilding);
                Building buildingTo = _context.Building.Single(e => e.IdBuilding == dtoToIdBuilding);

                if (buildingTo.City.Equals(buildingFrom.City) && buildingTo.Street.Equals(buildingFrom.Street))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<Banner> CalculateNewBanners(int dtoFromIdBuilding, int dtoToIdBuilding, double dtoPricePerSquareMeter)
        {
            try
            {
                Building buildingFrom = _context.Building.Single(e => e.IdBuilding == dtoFromIdBuilding);
                Building buildingTo = _context.Building.Single(e => e.IdBuilding == dtoToIdBuilding);

                int ileJeszczeBudynkow = Math.Abs(buildingTo.StreetNumber - buildingFrom.StreetNumber)-1;

                int minNumber;
                int maxNumber;
                int maxHeightNumber;
                double maxHeight;
                int secondMaxHeightNumber;
                double secondMaxHeight;

                if (buildingFrom.Height>buildingTo.Height)
                {
                    maxHeightNumber = buildingFrom.StreetNumber;
                    maxHeight = buildingFrom.Height;
                    secondMaxHeightNumber = buildingTo.StreetNumber;
                    secondMaxHeight = buildingTo.Height;
                }
                else
                {
                    maxHeightNumber = buildingTo.StreetNumber;
                    maxHeight = buildingTo.Height;
                    secondMaxHeightNumber = buildingFrom.StreetNumber;
                    secondMaxHeight = buildingFrom.Height;
                }


                if (buildingFrom.StreetNumber>buildingTo.StreetNumber)
                {
                    maxNumber = buildingFrom.StreetNumber;
                    minNumber = buildingTo.StreetNumber;
                }
                else
                {
                    maxNumber = buildingTo.StreetNumber;
                    minNumber = buildingFrom.StreetNumber;
                }

                for (int i = minNumber+1; i < maxNumber; i++)
                {
                    Building tmpBuilding = _context.Building.Single(e =>
                        e.StreetNumber == i && e.City.Equals(buildingFrom.City) &&
                        e.Street.Equals(buildingFrom.Street));

                    if (tmpBuilding.Height>maxHeight)
                    {
                        secondMaxHeightNumber = maxHeightNumber;
                        secondMaxHeight = maxHeight;
                        maxHeightNumber = tmpBuilding.StreetNumber;
                        maxHeight = tmpBuilding.Height;
                    }
                    else if (tmpBuilding.Height>secondMaxHeight)
                    {
                        secondMaxHeightNumber = tmpBuilding.StreetNumber;
                        secondMaxHeight = tmpBuilding.Height;
                    }
                }

                double banner1Height = 0;
                double banner2Height = 0;
                int howManyBuildingsB1 = 0;
                int howManyBuildingsB2 = 0;
                bool banner1Done = false;

                for (int i = minNumber; i <= maxNumber; i++)
                {
                    Building tmpBuilding = _context.Building.Single(e =>
                        e.StreetNumber == i && e.City.Equals(buildingFrom.City) &&
                        e.Street.Equals(buildingFrom.Street));

                    if (banner1Done)
                    {
                        if (tmpBuilding.StreetNumber==maxHeightNumber || tmpBuilding.StreetNumber == secondMaxHeightNumber)
                        {
                            banner2Height = tmpBuilding.Height;
                        }

                        howManyBuildingsB2++;
                    }
                    else
                    {
                        if (tmpBuilding.StreetNumber == maxHeightNumber || tmpBuilding.StreetNumber == secondMaxHeightNumber)
                        {
                            banner1Height = tmpBuilding.Height;
                            banner1Done = true;
                        }

                        howManyBuildingsB1++;
                    }
                }

                double b1Area = banner1Height * howManyBuildingsB1;
                double b2Area = banner2Height * howManyBuildingsB2;

                double b1Price = b1Area * dtoPricePerSquareMeter;
                double b2Price = b2Area * dtoPricePerSquareMeter;

                Banner b1 = new Banner {Area = b1Area, Price = b1Price, Name = 1};
                Banner b2 = new Banner {Area = b2Area, Price = b2Price, Name = 2};

                List<Banner> banners = new List<Banner>();
                banners.Add(b1);
                banners.Add(b2);
                return banners;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Campaign AddCampaignAndBanners(Campaign campaign, List<Banner> banners)
        {
            Campaign trackCampaign = campaign;
            _context.Campaign.Add(trackCampaign);
            _context.SaveChanges();
            int idCampaign = trackCampaign.IdCampaign;
            foreach (Banner banner in banners)
            {
                banner.IdCampaign = idCampaign;
                _context.Banner.Add(banner);
            }

            _context.SaveChanges();

            Campaign resCampaign = _context.Campaign.Include(e => e.Banners).Single(e => e.IdCampaign == idCampaign);
            return resCampaign;
        }

        public bool checkIfClientExists(string login)
        {
            var tmp = _context.Client.Where(e => e.Login.Equals(login)).ToList();
            if (tmp.Count>0)
            {
                return true;
            }

            return false;
        }

        public Client getClient(string login)
        {
            try
            {
                Client client = _context.Client.Single(e => e.Login.Equals(login));
                return client;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public string getSalt(int clientId)
        {
            try
            {
                Salt salt = _context.Salt.Single(e => e.IdClient == clientId);
                return salt.SaltHash;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public string getHashedPassword(int clientId)
        {
            try
            {
                Password password = _context.Password.Single(e => e.IdClient == clientId);
                return password.PasswordHash;
            }
            catch (Exception e)
            {
                return null;
            }
        }

    }
}
