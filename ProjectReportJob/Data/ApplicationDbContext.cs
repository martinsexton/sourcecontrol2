using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectReportJob.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Timesheet> Timesheet { get; set; }
        public DbSet<TimesheetEntry> TimesheetEntry { get; set; }
        public DbSet<TimesheetReport> TimesheetReport { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var connection =
                ConfigurationManager.ConnectionStrings["Database"].ConnectionString;

            options.UseSqlServer(connection, providerOptions => providerOptions.CommandTimeout(60));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Timesheet>()
                .HasMany(t => t.TimesheetEntries);
        }
    }
}
