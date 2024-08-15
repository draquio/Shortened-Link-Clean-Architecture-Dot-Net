using Microsoft.EntityFrameworkCore;
using ShortenedLinks.Domain.Entities;

namespace ShortenedLinks.Infrastructure.Persistence
{
    public class ShortenedLinksDbContext : DbContext
    {
        public ShortenedLinksDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Link> Links { get; set; } 
        public DbSet<LinkStatistic> LinksStatistics { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User
            modelBuilder.Entity<User>()
                .HasIndex(user => user.Email)
                .IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(user => user.Username)
                .IsUnique();

            // Link
            modelBuilder.Entity<Link>()
                .HasIndex(link => link.ShortenedLink)
                .IsUnique();
            modelBuilder.Entity<Link>()
                .HasOne(link => link.User)
                .WithMany(user => user.Links)
                .HasForeignKey(link => link.UserId);

            // LinkStatistic
            modelBuilder.Entity<LinkStatistic>()
                .HasOne(linkstatistic => linkstatistic.Link)
                .WithMany(link => link.LinkStatistics)
                .HasForeignKey(linkstatistic => linkstatistic.LinkId);
        }
    }
}
