using System;
using GameMiner.Core;

namespace Gameaways.Web.Model
{
    public class NotificationModel : BaseViewModel
    {
        public string Text { get; set; }
        public string Url { get; set; }
        public string NotificationDate { get; set; }
        public NotificationStatus Status { get; set; }
    }
}