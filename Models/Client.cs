using System.Collections.Generic;

namespace AdvertApi.Models
{
    public class Client
    {
        public Client()
        {
            ClientCampaigns = new HashSet<Campaign>();
        }

        public int IdClient { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Login { get; set; }
        public string? RefreshToken { get; set; }

        public Password Password { get; set; }
        public Salt Salt { get; set; }
        public ICollection<Campaign> ClientCampaigns { get; set; }
    }
}
