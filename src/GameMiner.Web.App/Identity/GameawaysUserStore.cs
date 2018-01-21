using GameMiner.DataLayer.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Threading;

namespace Gameaways.BusinessLayer.Identity
{
    public partial class GameawaysUserStore : IUserStore<User>, IUserRoleStore<User>, IUserLoginStore<User>, IUserSecurityStampStore<User>
    {
        private GameawaysDbContext _db;

        public GameawaysUserStore(GameawaysDbContext db)
        {
            _db = db;
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            _db.Users.Add(user);

            await _db.SaveChangesAsync();

            return IdentityResult.Success;
        }

        public Task<User> FindByNameAsync(string userName)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserName == userName);

            if (user != null)
            {
                return Task.FromResult(user);
            }
            else
            {
                return Task.FromResult<User>(null);
            }
        }

        public async Task UpdateAsync(User user)
        {
            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        Task IUserRoleStore<User>.AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task IUserRoleStore<User>.RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IList<string>> IUserRoleStore<User>.GetRolesAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult<IList<string>>(new List<string>());
        }

        Task<bool> IUserRoleStore<User>.IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        public Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            return Task.FromResult<IList<User>>(_db.Users.ToList());
        }

        Task<string> IUserStore<User>.GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        Task IUserStore<User>.SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.FromResult(true);
        }

        Task<string> IUserStore<User>.GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            return IdentityResult.Success;
        }

        Task<IdentityResult> IUserStore<User>.DeleteAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == long.Parse(userId));

            return Task.FromResult(user);
        }

        public Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserName == normalizedUserName);

            return Task.FromResult(user);
        }

        void IDisposable.Dispose()
        {
        }

        public async Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            _db.UserLogins.Add(new UserLogin
            {
                ProviderKey = login.ProviderKey,
                LoginProvider = login.LoginProvider,
                UserId = user.Id,
            });

            await _db.SaveChangesAsync();
        }

        public Task RemoveLoginAsync(User user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            var userLogin = _db.UserLogins.First(ul => ul.LoginProvider == loginProvider && ul.ProviderKey == providerKey);

            _db.UserLogins.Remove(userLogin);

            return Task.FromResult(true);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancellationToken)
        {
            var result = _db.UserLogins
                .Where(ul => ul.UserId == user.Id)
                .Select(ul => new UserLoginInfo(ul.LoginProvider, ul.ProviderKey, ul.LoginProvider))
                .ToList();

            return await Task.FromResult<IList<UserLoginInfo>>(result);
        }

        public async Task<User> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            var userLogin = _db.UserLogins.FirstOrDefault(ul => ul.LoginProvider == loginProvider && ul.ProviderKey == providerKey);

            if (userLogin != null)
            {
                var result = _db.Users.Find(userLogin.UserId);

                return await Task.FromResult(result);
            }
            else
            {
                return await Task.FromResult<User>(null);
            }
        }

        public Task SetSecurityStampAsync(User user, string stamp, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        public Task<string> GetSecurityStampAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }
    }
}