using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Planora.DataAccess.Models;

namespace Planora.DataAccess.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
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


        //Insert DBset for each table here 
        public DbSet<CategoryDB> Categories { get; set; }
        internal DbSet<TaskDB> Tasks { get; set; }
    }
}
