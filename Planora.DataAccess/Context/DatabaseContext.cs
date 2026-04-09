using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Planora.DataAccess
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

        internal DbSet<UserDB> Users { get; set; }
        //Insert DBset for each table here 
    }
}
