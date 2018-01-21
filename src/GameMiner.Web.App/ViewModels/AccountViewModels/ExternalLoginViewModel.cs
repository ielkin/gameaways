using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameMiner.Web.App.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        public string Username { get; set; }

        public string ProfileImageUrl { get; set; }
    }
}
