using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertApi.Models
{
    public class Salt
    {
        public int IdSalt { get; set; }
        public int IdClient { get; set; }
        public string SaltHash { get; set; }

        public Client Client { get; set; }
    }
}
