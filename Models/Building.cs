using System.Collections.Generic;

namespace AdvertApi.Models
{
    public class Building
    {
        public Building()
        {
            FromBuildingCampaigns = new HashSet<Campaign>();
            ToBuildingCampaigns = new HashSet<Campaign>();
        }
        public int IdBuilding { get; set; }
        public string Street { get; set; }
        public int StreetNumber { get; set; }
        public string City { get; set; }
        public double Height { get; set; }

        public ICollection<Campaign> FromBuildingCampaigns { get; set; }
        public ICollection<Campaign> ToBuildingCampaigns { get; set; }
    }
}
