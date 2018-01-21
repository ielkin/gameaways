using System;
using System.ComponentModel.DataAnnotations;

namespace GameMiner.Web.App.ViewModels
{
    public class GiveawayModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "GameId")]
        public int GameId { get; set; }

        public string Description { get; set; }

        [Display(Name = "DurationInDays")]
        public int DurationInDays { get; set; }
    }
}