using Microsoft.EntityFrameworkCore;
using Planora.DataAccess.Models;

namespace Planora.DataAccess.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected DatabaseContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Create associations between classes
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Update to use SQLServer when going in production
        }

        public DbSet<UserDB> Users { get; set; }
        public DbSet<CategoryDB> Categories { get; set; }
        internal DbSet<TaskDB> Tasks { get; set; }
        internal DbSet<ProjectDB> Projects { get; set; }
    }
}
