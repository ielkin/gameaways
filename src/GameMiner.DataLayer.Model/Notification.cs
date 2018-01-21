using System;
using System.Collections.Generic;
using GameMiner.Core;

namespace GameMiner.DataLayer.Model
{
    public class Notification : BaseUserModel
    {
        public string Text { get; set; }
        public string Url { get; set; }
        public NotificationStatus Status { get; set; }
        public DateTime NotificationDate { get; set; }
    }
}
