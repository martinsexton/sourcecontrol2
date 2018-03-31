﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CharliesApplication.Models;

namespace CharliesApplication.DataAccess
{
    public class BabyContext : DbContext
    {
        public BabyContext(DbContextOptions<BabyContext> options)
         : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Ensure private property on Baby gets mapped to database
            modelBuilder.Entity<Baby>().Property(
            typeof(DateTime), "CreatedDate").IsRequired();

            //Model the one to one relationship between Baby and Birth Details
            modelBuilder.Entity<BirthDetails>()
                .HasKey(b => b.BabyId);
            modelBuilder.Entity<BirthDetails>()
              .HasOne(b => b.Baby)
              .WithOne(b => b.BirthDetails)
              .IsRequired();
            modelBuilder
              .Entity<Baby>()
              .HasOne(b => b.BirthDetails)
              .WithOne(d => d.Baby).OnDelete
                (DeleteBehavior.Cascade);

            modelBuilder
                .Entity<Appointment>()
                .HasOne(p => p.Baby)
                .WithMany(b => b.Appointments)
                .IsRequired();

            modelBuilder
               .Entity<Activity>()
               .HasOne(p => p.Baby)
               .WithMany(b => b.Activities)
               .IsRequired();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<Baby> Baby { get; set; }
        public DbSet<Activity> Activity { get; set; }
        public DbSet<Project> Project { get; set; }
    }
}