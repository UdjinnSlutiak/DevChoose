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
    }
}
