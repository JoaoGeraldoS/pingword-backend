using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using pingword.src.Models.FeedBacks;
using pingword.src.Models.Notifications;
using pingword.src.Models.StudyState;
using pingword.src.Models.Users;
using pingword.src.Models.Words;

namespace pingword.src.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Study> Studies { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
        public DbSet<Word> Words { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Notification>()
                .Property(n => n.NotificationEnum)
                .HasConversion<string>();

            builder.Entity<Study>()
                .Property(s => s.Status)
                .HasConversion<string>();

            builder.Entity<User>()
                .HasOne(u => u.StudyState)
                .WithOne(s => s.User)
                .HasForeignKey<Study>(s => s.UserId);

            builder.Entity<User>()
                .HasMany(u => u.Notifications)
                .WithOne(n => n.User)
                .HasForeignKey(n => n.UserId);

            builder.Entity<User>()
                .HasMany(u => u.Words)
                .WithOne(w => w.Users)
                .HasForeignKey(w => w.UserId);

        }

    }
}
