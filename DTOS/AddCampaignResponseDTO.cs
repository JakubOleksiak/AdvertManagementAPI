using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdvertApi.Models;

namespace AdvertApi.DTOS
{
    public class AddCampaignResponseDTO
    {
        public Campaign Campaign { get; set; }
        public double FullPrice { get; set; }
    }
}
