using GameCenter.Models;
using Microsoft.EntityFrameworkCore;

namespace GameCenter.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<GameToPlatform> GameToPlatform { get; set; }
    }
}
