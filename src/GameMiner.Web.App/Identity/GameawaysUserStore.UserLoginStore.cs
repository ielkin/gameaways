//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web;
//using Gameaways.DataLayer.Database;
//using Gameaways.DataLayer.Model;
//using Microsoft.AspNet.Identity;

//namespace Gameaways.BusinessLayer.Identity
//{
//    public partial class GameawaysUserStore : IUserLoginStore<UserIdentityModel, int>
//    {
//        public async Task AddLoginAsync(UserIdentityModel user, UserLoginInfo login)
//        {
//            db.UserLogins.Add(new UserLogin()
//            {
//                UserId = user.Id,
//                LoginProvider = login.LoginProvider,
//                ProviderKey = login.ProviderKey,
//            });

//            await db.SaveChangesAsync();
//        }

//        public async Task<UserIdentityModel> FindAsync(UserLoginInfo login)
//        {
//            var externalUser = db.UserLogins.FirstOrDefault(ul => ul.LoginProvider == login.LoginProvider && ul.ProviderKey == login.ProviderKey);

//            if (externalUser != null)
//            {
//                return await FindByIdAsync(externalUser.UserId);
//            }

//            return await Task.FromResult<UserIdentityModel>(null);
//        }

//        public async Task<IList<UserLoginInfo>> GetLoginsAsync(UserIdentityModel user)
//        {
//            IList<UserLoginInfo> logins = new List<UserLoginInfo>();

//            var userLogins = db.UserLogins.Where(oau => oau.UserId == user.Id);

//            foreach (var login in userLogins)
//            {
//                logins.Add(new UserLoginInfo(login.LoginProvider, login.ProviderKey));
//            }

//            return await Task.FromResult<IList<UserLoginInfo>>(logins);
//        }

//        public async Task RemoveLoginAsync(UserIdentityModel user, UserLoginInfo login)
//        {
//            var userLogin = await db.UserLogins.FirstAsync(ul => ul.UserId == user.Id && ul.LoginProvider == login.LoginProvider);

//            db.UserLogins.Remove(userLogin);

//            await db.SaveChangesAsync();
//        }
//    }
//}