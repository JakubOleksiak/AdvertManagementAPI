using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertApi.Models
{
    public class Password
    {
        public int IdPassword { get; set; }
        public int IdClient { get; set; }
        public string PasswordHash { get; set; }

        public Client Client { get; set; }
    }
}
