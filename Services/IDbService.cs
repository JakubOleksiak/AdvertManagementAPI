using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdvertApi.DTOS;
using AdvertApi.Models;

namespace AdvertApi.Services
{
    public interface IDbService
    {
        public Client RegisterClient(RegisterClientDTO dto);
        public Client GetClientFromRefreshToken(string refToken);
        public string SetRefreshToken(string clientLogin, string refToken);
        public bool checkIfClientExists(string login);
        Client getClient(string login);
        string getSalt(int clientId);
        string getHashedPassword(int clientId);
        List<Campaign> getCampaigns();
        bool checkBuildings(int dtoFromIdBuilding, int dtoToIdBuilding);
        List<Banner> CalculateNewBanners(int dtoFromIdBuilding, int dtoToIdBuilding, double dtoPricePerSquareMeter);
        Campaign AddCampaignAndBanners(Campaign campaign, List<Banner> banners);
    }
}
