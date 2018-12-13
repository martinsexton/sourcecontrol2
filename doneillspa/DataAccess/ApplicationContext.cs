﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace doneillspa.DataAccess
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }

        public DbSet<Project> Project { get; set; }
        public DbSet<Certification> Certification { get; set; }
        public DbSet<EmailNotification> EmailNotification { get; set; }
        public DbSet<Timesheet> Timesheet { get; set; }
        public DbSet<TimesheetEntry> TimesheetEntry { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Timesheet>()
                .HasMany(t => t.TimesheetEntries);

            modelBuilder.Entity<Certification>()
                .HasOne<ApplicationUser>(n => n.User)
                .WithMany(a => a.Certifications)
                .HasForeignKey(n => n.UserId);

            modelBuilder.Entity<EmailNotification>()
               .HasOne<ApplicationUser>(n => n.User)
               .WithMany(a => a.EmailNotifications)
               .HasForeignKey(n => n.UserId);

            modelBuilder.Entity<Notification>()
                .ToTable("Notifications")
                .HasDiscriminator<int>("Type")
                .HasValue<EmailNotification>(1);
        }
    }
}
