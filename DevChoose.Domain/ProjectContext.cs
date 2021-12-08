using DevChoose.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DevChoose.Domain
{
    public class ProjectContext : DbContext
    {
        public ProjectContext(DbContextOptions<ProjectContext> contextOptions)
            : base(contextOptions)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Technology> Technologies { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Dialog> Dialogs { get; set; }
        public DbSet<UsersTechnologies> UsersTechnologies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(m => m.SenderId);

            modelBuilder.Entity<Message>()
                .HasOne<Dialog>()
                .WithMany()
                .HasForeignKey(m => m.DialogId);

            modelBuilder.Entity<Message>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<Dialog>()
                .HasKey(d => d.Id);

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<Technology>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<UsersTechnologies>()
                .HasKey(_ => new { _.UserId, _.TechnologyId });
        }
    }
}
