using System;
using System.Collections;
using System.Collections.Generic;

namespace GameMiner.DataLayer.Model
{
    public class UserLogin : BaseUserModel
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
    }
}