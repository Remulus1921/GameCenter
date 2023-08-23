using GameCenter.Models;
using GameCenter.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameCenter.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Game> Games { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Platform> Platforms { get; set; }
    public DbSet<Rate> Rates { get; set; }
    public DbSet<GameToPlatform> GameToPlatform { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostToPlatform> PostToPlatforms { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());

        builder.Entity<Rate>()
            .HasOne(r => r.Game)
            .WithMany(g => g.GameRates)
            .HasForeignKey(r => r.GameId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Comment>()
            .HasOne(c => c.Parent)
            .WithMany(c => c.Replies)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Comment>()
            .HasOne(c => c.Game)
            .WithMany(g => g.GameComments)
            .HasForeignKey(c => c.GameId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Game>()
            .HasMany(g => g.GameRates)
            .WithOne(r => r.Game)
            .HasForeignKey(r => r.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Game>()
            .HasMany(g => g.GameComments)
            .WithOne(r => r.Game)
            .HasForeignKey(c => c.GameId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}

internal class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<GameCenterUser>
{
    public void Configure(EntityTypeBuilder<GameCenterUser> builder)
    {
        builder.Property(u => u.FirstName).HasMaxLength(128);
        builder.Property(u => u.LastName).HasMaxLength(128);
    }
}
