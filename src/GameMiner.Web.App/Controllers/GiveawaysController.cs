using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using Gameaways.Web.Model;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GameMiner.BusinessLayer;
using GameMiner.DataLayer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using GameMiner.Web.App.ViewModels;
using GameMiner.BusinessLayer.Core;
using Microsoft.Extensions.Caching.Memory;

namespace GameMiner.Web.Controllers
{
    public class GiveawaysController : Controller
    {
        private readonly IMessagePublisher _messagePublisher;
        private readonly ICoinhiveApiClient _coinhiveApi;
        private GiveawayService _giveawayService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly GameawaysDbContext _db;
        private readonly IStringLocalizer<GiveawaysController> _localizer;
        private IMemoryCache _memoryCache;
        private int _userId;

        public GiveawaysController(GameawaysDbContext db,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IStringLocalizer<GiveawaysController> localizer,
            IMemoryCache memoryCache,
            ICoinhiveApiClient coinhiveApi,
            IMessagePublisher messagePublisher)
        {
            _messagePublisher = messagePublisher;
            _giveawayService = new GiveawayService(db);
            _userManager = userManager;
            _signInManager = signInManager;
            _localizer = localizer;
            _db = db;
            _memoryCache = memoryCache;
            _coinhiveApi = coinhiveApi;
        }

        public async Task<ActionResult> Index(int? page)
        {
            int currentPage = page ?? 1;

            int itemsPerPage = 10;

            GiveawayListModel model = new GiveawayListModel();

            var giveaways = _db.Giveaways
                .Where(ga => ga.EndDate > DateTime.UtcNow && ga.StartDate < DateTime.UtcNow)
                .OrderByDescending(ga => ga.StartDate);

            model.Giveaways = await giveaways
                .Select(g => new GiveawayListViewModel()
                {
                    Description = g.Description,
                    EndDate = g.EndDate,
                    Id = g.Id,
                    GameId = g.GameId,
                    TileUrl = g.Game.HeaderImageUrl,
                    StartDate = g.StartDate,
                    Title = g.Game.Title,
                    Username = g.User.UserName,
                    ProfilePictureUrl = g.User.ProfilePictureUrl,
                    FundingProgress = (int)g.Game.Price,
                })
                .ToListAsync();

            foreach (var ga in model.Giveaways)
            {
                ga.Title = ga.Title.Truncate(40);

                if (ga.NumberOfCopies > 1)
                {
                    ga.Title += string.Format(" ({0})", _localizer["Copy"].ToString().ToQuantity(ga.NumberOfCopies));
                }
            }

            foreach (var ga in model.Giveaways)
            {
                ga.EntriesCount = await _db.GiveawayEntries.Where(ge => ge.GiveawayId == ga.Id).CountAsync();

                ga.FundingProgress = (int)((double)ga.EntriesCount / (double)ga.FundingProgress * 100 / 40);
                ga.FundingProgress = ga.FundingProgress < 100 ? ga.FundingProgress : 100;

                if (_signInManager.IsSignedIn(User))
                {
                    _userId = int.Parse(_userManager.GetUserId(User));

                    ga.CurrentUserEntriesCount = await _db.GiveawayEntries.Where(ge => ge.GiveawayId == ga.Id && ge.UserId == _userId).CountAsync();
                }
            }

            model.ItemsCount = model.Giveaways.Count();

            model.Giveaways = model.Giveaways
                    .Skip((currentPage - 1) * itemsPerPage)
                    .Take(itemsPerPage)
                    .ToList();

            model.CurrentPage = currentPage;

            double pagesCount = (double)model.ItemsCount / (double)itemsPerPage;

            pagesCount = Math.Ceiling(pagesCount);

            model.PagesCount = (int)pagesCount;

            return View(model);
        }

        public async Task<ActionResult> ViewGiveaway(int id)
        {
            Giveaway giveaway = await _db.Giveaways.FirstAsync(ga => ga.Id == id);
            User user = _db.Users.First(u => u.Id == giveaway.UserId);
            Game game = _db.Games.First(g => g.Id == giveaway.GameId);

            GiveawayViewModel model = new GiveawayViewModel()
            {
                Description = giveaway.Description,
                StartDate = giveaway.StartDate,
                EndDate = giveaway.EndDate,
                GameId = giveaway.GameId,
                Id = giveaway.Id,
                TileUrl = game.LargeHeaderImageUrl,
                Status = giveaway.Status,
                Title = game.Title,
                Username = user.UserName,
                ProfilePictureUrl = user.ProfilePictureUrl,
                EntryStatus = EntryStatus.Enabled,
            };

            if (model.NumberOfCopies > 1)
            {
                model.Title = model.Title.Truncate(50) + string.Format(" ({0})", _localizer["Copy"].ToString().ToQuantity(model.NumberOfCopies));
            }

            model.EntriesCount = _db.GiveawayEntries.Count(ge => ge.GiveawayId == model.Id);

            model.FundingProgress = (int)((double)model.EntriesCount / game.Price * 100 / 40);

            model.FundingProgress = model.FundingProgress < 100 ? model.FundingProgress : 100;

            if (model.EndDate < DateTime.UtcNow)
            {
                model.EntryStatus = EntryStatus.Ended;
            }

            if (_signInManager.IsSignedIn(User))
            {
                long userId = int.Parse(_userManager.GetUserId(User));
                User currentUser = _db.Users.First(u => u.Id == userId);

                model.CurrentUserEntriesCount = _db.GiveawayEntries.Count(ge => ge.GiveawayId == id && ge.UserId == userId);

                if (currentUser.CreditBalance == 0)
                {
                    model.EntryStatus = EntryStatus.InsufficientBalance;
                }
            }

            return View(model);
        }

        public JsonResult Enter(long id, string token)
        {
            long userId = long.Parse(_userManager.GetUserId(User));

            var user = _db.Users.First(u => u.Id == userId);
            var giveaway = _db.Giveaways.First(ga => ga.Id == id);
            var game = _db.Games.First(g => g.Id == giveaway.GameId);

            bool entriesEnabled = true;
            string entryError = string.Empty;

            if (DateTime.UtcNow > giveaway.EndDate)
            {
                entriesEnabled = false;
                entryError = "Closed";
            }

            if (entriesEnabled && user.CreditBalance > 0)
            {
                user.CreditBalance -= 1;

                for (int i = 0; i < 10; i++)
                {
                    _db.GiveawayEntries.Add(new GiveawayEntry()
                    {
                        EntryDate = DateTime.UtcNow,
                        GiveawayId = id,
                        UserId = userId,
                    });
                }

                _db.SaveChanges();
            }
            else
            {
                bool tokenValid = _coinhiveApi.VerifyToken(token, 2560);

                if (tokenValid)
                {
                    _db.GiveawayEntries.Add(new GiveawayEntry()
                    {
                        EntryDate = DateTime.UtcNow,
                        GiveawayId = id,
                        UserId = userId,
                    });

                    _db.SaveChanges();
                }
            }

            if (entriesEnabled && user.CreditBalance == 0)
            {
                entriesEnabled = false;
                entryError = "Not Enough Credits";
            }

            int userEntries = _db.GiveawayEntries.Count(ge => ge.GiveawayId == id && ge.UserId == userId);
            int totalEntries = _db.GiveawayEntries.Count(ge => ge.GiveawayId == id);

            int fundingProgress = (int)((double)totalEntries / game.Price * 100 / 40);

            return Json(new
            {
                EntriesEnabled = entriesEnabled,
                EntryError = entryError,
                LeaveButtonLabel = _localizer["Remove"] + " " + _localizer["Entry"].ToString().ToQuantity(userEntries),
                EntriesCount = "Entry".ToQuantity(totalEntries),
                UserEntries = "Entry".ToQuantity(userEntries),
                UserEntriesCount = userEntries,
                FundingProgress = fundingProgress < 100 ? fundingProgress : 100,
                CreditBalance = user.CreditBalance,
            });
        }

        public JsonResult Leave(long id, string token)
        {
            long userId = long.Parse(_userManager.GetUserId(User));

            var giveaway = _db.Giveaways.First(ga => ga.Id == id);
            var game = _db.Games.First(g => g.Id == giveaway.GameId);
            var user = _db.Users.First(u => u.Id == userId);

            if (giveaway.EndDate > DateTime.UtcNow)
            {
                var entries = _db.GiveawayEntries.Where(ge => ge.GiveawayId == id && ge.UserId == userId);
                
                user.CreditBalance += entries.Count() / 10;
                _db.GiveawayEntries.RemoveRange(entries);

                _db.SaveChanges();
            }

            int totalEntries = _db.GiveawayEntries.Count(ge => ge.GiveawayId == id);

            int fundingProgress = (int)((double)totalEntries / game.Price * 100 / 40);

            return Json(new
            {
                CreditBalance = user.CreditBalance,
                EntriesCount = "Entry".ToQuantity(totalEntries),
                FundingProgress = fundingProgress,
                EntriesEnabled = user.CreditBalance > 0,
            });
        }

        public JsonResult GetEntries(int giveawayId, int page, int pageSize)
        {

            var entriesToDisplay = new List<GiveawayEntryModel>();

            var entries = _db.GiveawayEntries.Where(ge => ge.GiveawayId == giveawayId).OrderByDescending(ge => ge.EntryDate);

            double pagesCount = Math.Ceiling((double)entries.Count() / (double)pageSize);

            foreach (var entry in entries.Skip(page * pageSize).Take(pageSize).ToList())
            {
                entriesToDisplay.Add(new GiveawayEntryModel()
                {
                    Id = entry.Id,
                    EntryDate = entry.EntryDate.Humanize(),
                    ProfilePictureUrl = entry.User.ProfilePictureUrl,
                    Username = entry.User.UserName,
                });
            }

            return Json(new
            {
                PagesCount = pagesCount,
                Entries = entriesToDisplay
            });
        }

        [Authorize]
        public async Task<JsonResult> SendGift(int winnerId, string redemptionCode)
        {
            await _giveawayService.SendGiftAsync(winnerId, redemptionCode);

            return Json(new
            {
                Result = true,
                Description = "Gift Sent",
            });
        }

        [Authorize]
        public async Task<JsonResult> PickNewWinner(int winnerId)
        {
            await _giveawayService.PickNewWinnerAsync(winnerId);


            var winner = await _db.GiveawayWinners.FirstOrDefaultAsync(gw => gw.Id == winnerId);

            return Json(new
            {
                Username = winner.User.UserName,
                GiftStatus = "Gift " + winner.GiftStatus.Humanize(),
                Result = true,
            });

        }

        [Authorize(Policy = "Gameaways")]
        public ActionResult Create()
        {
            int userId = int.Parse(_userManager.GetUserId(User));

            var model = new GiveawayModel()
            {
                DurationInDays = 45
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "Gameaways")]
        public async Task<ActionResult> Create(GiveawayModel model)
        {
            if (ModelState.IsValid && model.GameId > 0)
            {
                int userId = int.Parse(_userManager.GetUserId(User));

                string descriptionText = HttpUtility.HtmlEncode(model.Description);

                if (!string.IsNullOrEmpty(model.Description))
                {
                    descriptionText = descriptionText.Replace("\n", "</br>");
                }

                var ga = new Giveaway()
                {
                    Description = descriptionText,
                    EndDate = DateTime.UtcNow.AddDays(model.DurationInDays),
                    StartDate = DateTime.UtcNow,
                    Status = GiveawayStatus.Open,
                    GameId = model.GameId,
                    UserId = userId,
                };

                _db.Giveaways.Add(ga);
                await _db.SaveChangesAsync();

                _messagePublisher.Publish(ga.Id.ToString(), "giveaway-ended", ga.EndDate);

                return Redirect($"/Giveaway/{ ga.Id }");
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult SearchGames(string term, bool exactMatch = false)
        {
            Expression<Func<Game, bool>> searchExpression = (game) => game.Title.Contains(term);

            if (exactMatch)
            {
                searchExpression = (game) => game.Title == term;
            }

            var games = _db.Games
                .Where(searchExpression)
                .Select(item => new
                {
                    id = item.Id,
                    label = item.Title,
                    value = item.Title,
                    imageUrl = item.SmallHeaderImageUrl,
                })
                .ToList();

            if (games.Count() == 0)
            {
                var noResults = new[] { new { Id = 0, label = "Nothing was found :(", value = "Nothing was found :(", imageUrl = "" } };

                return Json(noResults);
            }
            else
            {
                return Json(games);
            }
        }
    }
}
