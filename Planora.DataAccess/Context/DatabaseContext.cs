using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Planora.DataAccess.Models;
using Planora.DataAccess.Models.Auth;

namespace Planora.DataAccess.Context
{
    public sealed class DatabaseContext : IdentityDbContext<AuthUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);  // Sets up Identity tables and relationships
            
            modelBuilder.Entity<AuthUser>(entity => 
            {
                entity.HasOne(a => a.UserDb)
                    .WithOne() 
                    .HasForeignKey<AuthUser>(a => a.UserDBId);

                // ALWAYS fetch the UserDb profile when fetching an AuthUser
                entity.Navigation(a => a.UserDb)
                    .AutoInclude();
            });
        }
        
        public DbSet<UserDB> Users { get; set; }
        
        public DbSet<CategoryDB> Categories { get; set; }
        public DbSet<TaskDB> Tasks { get; set; }
        public DbSet<ProjectDB> Projects { get; set; }
    }
}
