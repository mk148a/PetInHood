using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalProtect.Models
{
    public class CampaignPost
    {
        public int id { get; set; }
        public string[] photos { get; set; }
        public string creatorUsername { get; set; }
        public string photoOfcreator { get; set; }
        public DateTime createdAt { get; set; }
        public string tag { get; set; }
        public string description { get; set; }
        public int likedUserscount { get; set; }
        public int commentsCount { get; set; }
        public int donatedMoney { get; set; }
        public int offeredMoney { get; set; }
        public bool isDonationactive { get; set; }
        public string location { get; set; }

    }
}
