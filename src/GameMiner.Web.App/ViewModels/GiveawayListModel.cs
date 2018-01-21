using System;
using System.Collections.Generic;

namespace Gameaways.Web.Model
{
    public class GiveawayListModel
    {
        public IList<GiveawayListViewModel> Giveaways { get; set; }
        public int ItemsCount { get; set; }
        public int PagesCount { get; set; }
        public int CurrentPage { get; set; }

        public GiveawayListModel()
        {
            Giveaways = new List<GiveawayListViewModel>();
        }
    }
}