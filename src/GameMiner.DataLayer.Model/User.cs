using System;
using System.Collections.Generic;

namespace GameMiner.DataLayer.Model
{
    public class User : BaseModel
    {
        public string UserName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public DateTime RegistrationDate { get; set; }
        public long CreditBalance { get; set; }
    }
}
