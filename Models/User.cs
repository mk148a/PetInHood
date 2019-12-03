using System;
using System.Collections.Generic;

namespace AnimalProtect.Models
{
    public partial class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string eMailAddress { get; set; }
        public string mobileId { get; set; }
        public string profilePhoto { get; set; }
        public int petCoin { get; set; }
        public string badge { get; set; }
        public string postIds { get; set; }
        public string following { get; set; }
        public string followers { get; set; }
        public string firebaseToken { get; set; }


    }
}
