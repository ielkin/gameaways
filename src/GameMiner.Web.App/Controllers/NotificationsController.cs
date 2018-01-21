using System;
using System.Collections.Generic;
using System.Linq;
using Gameaways.Web.Model;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using GameMiner.DataLayer.Model;

namespace GameMiner.Web.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly GameawaysDbContext _db;
        private int _userId;

        public NotificationsController(GameawaysDbContext db, UserManager<User> userManager)
        {
            _userManager = userManager;
            _db = db;

            _userId = int.Parse(_userManager.GetUserId(User));
        }

        public JsonResult Dismiss(int id)
        {
            var dismissedNotification = _db.Notifications.First(n => n.Id == id);

            _db.Notifications.Remove(dismissedNotification);
            _db.SaveChanges();

            return Json(new { Status = true });
        }

        public ActionResult NotificationsView()
        {
            int userId = int.Parse(_userManager.GetUserId(User));

            ViewBag.HasNewNotifications = _db.Notifications.Any(n => n.UserId == _userId);

            return PartialView("Partial/Notifications");
        }

        public JsonResult GetNotifications()
        {
            IList<NotificationModel> notifications;

            notifications = _db.Notifications
                .Where(un => un.UserId == _userId)
                .OrderByDescending(n => n.NotificationDate)
                .ToList()
                .Select(n => new NotificationModel()
                {
                    Id = n.Id,
                    NotificationDate = DateTime.UtcNow.Subtract(n.NotificationDate).Humanize() + " ago",
                    Status = n.Status,
                    Text = n.Text,
                    Url = n.Url,
                })
                .ToList();

            return Json(notifications);
        }
    }
}
