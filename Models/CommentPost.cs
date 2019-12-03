using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalProtect.Models
{
    public class CommentPost
    {
        //automated give from server
        public int id { get; set; }
        //necessary field
        public int campaignPostid { get; set; }
        //default 0
        public int mentionedId { get; set; }
        //necessary field
        public string creatorUsername { get; set; }
        //automated give from server
        public DateTime createdAt { get; set; }
        //necessary field
        public string description { get; set; }
        //automated give from server when this comment liked
        public int likedUserscount { get; set; }       

    }
}
