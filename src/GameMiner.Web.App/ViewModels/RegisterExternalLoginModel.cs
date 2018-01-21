using System;

namespace Gameaways.Web.Model
{
    public class ExternalLoginModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ProviderName { get; set; }
        public string UserLogoUrl { get; set; }
    }
}