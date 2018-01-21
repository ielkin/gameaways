using System;

namespace Gameaways.Web.Model
{
    public class BaseUserViewModel : BaseViewModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}