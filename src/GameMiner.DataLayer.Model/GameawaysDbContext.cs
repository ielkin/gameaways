using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.EntityFrameworkCore;

namespace GameMiner.DataLayer.Model
{
    public class GameawaysDbContext : DbContext
    {
        public GameawaysDbContext(DbContextOptions<GameawaysDbContext> options)
            : base(options)
        {
        }

        public DbSet<ExcludedSteamGame> ExcludedStoreGames { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Giveaway> Giveaways { get; set; }
        public DbSet<GiveawayEntry> GiveawayEntries { get; set; }
        public DbSet<GiveawayWinner> GiveawayWinners { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
    }
}
