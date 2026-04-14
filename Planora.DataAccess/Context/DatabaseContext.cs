using Microsoft.EntityFrameworkCore;
using Planora.DataAccess.Models;
using Planora.DataAccess.Models.Auth;

namespace Planora.DataAccess.Context
{
    public sealed class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        private DatabaseContext()
        {
		    Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Create associations between classes
        }
        
        public DbSet<UserDB> Users { get; set; }
        public DbSet<AuthUser> AuthUsers { get; set; }
        public DbSet<CategoryDB> Categories { get; set; }
        public DbSet<TaskDB> Tasks { get; set; }
        public DbSet<ProjectDB> Projects { get; set; }
    }
}
