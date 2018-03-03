﻿// <auto-generated />
using CharliesApplication.DataAccess;
using CharliesApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace CharliesApplication.Migrations
{
    [DbContext(typeof(BabyContext))]
    [Migration("20180303224649_appointmenttypeadded")]
    partial class appointmenttypeadded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CharliesApplication.Models.Appointment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("BabyId")
                        .IsRequired();

                    b.Property<string>("Description");

                    b.Property<DateTime>("DueDate");

                    b.Property<string>("Outcome");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("BabyId");

                    b.ToTable("Appointment");
                });

            modelBuilder.Entity("CharliesApplication.Models.Baby", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("FirstName");

                    b.Property<string>("Sex");

                    b.Property<string>("Surname");

                    b.HasKey("Id");

                    b.ToTable("Baby");
                });

            modelBuilder.Entity("CharliesApplication.Models.BirthDetails", b =>
                {
                    b.Property<long>("BabyId");

                    b.Property<DateTime>("BirthDate");

                    b.Property<string>("Hospital");

                    b.Property<decimal>("Weight");

                    b.HasKey("BabyId");

                    b.ToTable("BirthDetails");
                });

            modelBuilder.Entity("CharliesApplication.Models.Appointment", b =>
                {
                    b.HasOne("CharliesApplication.Models.Baby", "Baby")
                        .WithMany("Appointments")
                        .HasForeignKey("BabyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CharliesApplication.Models.BirthDetails", b =>
                {
                    b.HasOne("CharliesApplication.Models.Baby", "Baby")
                        .WithOne("BirthDetails")
                        .HasForeignKey("CharliesApplication.Models.BirthDetails", "BabyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
