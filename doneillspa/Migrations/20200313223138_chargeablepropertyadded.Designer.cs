﻿// <auto-generated />
using doneillspa.DataAccess;
using doneillspa.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;

namespace doneillspa.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20200313223138_chargeablepropertyadded")]
    partial class chargeablepropertyadded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("doneillspa.Models.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<bool?>("IsEnabled");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Surname");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("doneillspa.Models.Certification", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Description");

                    b.Property<DateTime>("Expiry");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Certification");
                });

            modelBuilder.Entity("doneillspa.Models.Client", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("doneillspa.Models.HolidayRequest", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ApproverId");

                    b.Property<int>("Days");

                    b.Property<DateTime>("FromDate");

                    b.Property<DateTime>("RequestedDate");

                    b.Property<int>("Status");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ApproverId");

                    b.HasIndex("UserId");

                    b.ToTable("HolidayRequest");
                });

            modelBuilder.Entity("doneillspa.Models.LabourRate", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EffectiveFrom");

                    b.Property<DateTime?>("EffectiveTo");

                    b.Property<double>("OverTimeRatePerHour");

                    b.Property<double>("RatePerHour");

                    b.Property<string>("Role");

                    b.HasKey("Id");

                    b.ToTable("LabourRate");
                });

            modelBuilder.Entity("doneillspa.Models.NonChargeableTime", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("Description");

                    b.HasKey("Id");

                    b.ToTable("NonChargeableTime");
                });

            modelBuilder.Entity("doneillspa.Models.Notification", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ActivationDate");

                    b.Property<int>("Type");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.ToTable("Notifications");

                    b.HasDiscriminator<int>("Type");
                });

            modelBuilder.Entity("doneillspa.Models.Project", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("Details");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name");

                    b.Property<long>("OwningClientId");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("OwningClientId");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("doneillspa.Models.Timesheet", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<Guid>("Owner");

                    b.Property<string>("Role");

                    b.Property<int>("Status");

                    b.Property<string>("Username");

                    b.Property<DateTime>("WeekStarting");

                    b.HasKey("Id");

                    b.ToTable("Timesheet");
                });

            modelBuilder.Entity("doneillspa.Models.TimesheetEntry", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Chargeable");

                    b.Property<string>("Code");

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("Day");

                    b.Property<string>("Details");

                    b.Property<string>("EndTime");

                    b.Property<string>("StartTime");

                    b.Property<long?>("TimesheetId");

                    b.HasKey("Id");

                    b.HasIndex("TimesheetId");

                    b.ToTable("TimesheetEntry");
                });

            modelBuilder.Entity("doneillspa.Models.TimesheetNote", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("Details");

                    b.Property<long?>("TimesheetId");

                    b.HasKey("Id");

                    b.HasIndex("TimesheetId");

                    b.ToTable("TimesheetNote");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<Guid>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("doneillspa.Models.EmailNotification", b =>
                {
                    b.HasBaseType("doneillspa.Models.Notification");

                    b.Property<string>("Body");

                    b.Property<string>("DestinationEmail");

                    b.Property<string>("Subject");

                    b.HasIndex("UserId");

                    b.ToTable("EmailNotification");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("doneillspa.Models.Certification", b =>
                {
                    b.HasOne("doneillspa.Models.ApplicationUser", "User")
                        .WithMany("Certifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("doneillspa.Models.HolidayRequest", b =>
                {
                    b.HasOne("doneillspa.Models.ApplicationUser", "Approver")
                        .WithMany()
                        .HasForeignKey("ApproverId");

                    b.HasOne("doneillspa.Models.ApplicationUser", "User")
                        .WithMany("HolidayRequests")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("doneillspa.Models.Project", b =>
                {
                    b.HasOne("doneillspa.Models.Client", "OwningClient")
                        .WithMany("Projects")
                        .HasForeignKey("OwningClientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("doneillspa.Models.TimesheetEntry", b =>
                {
                    b.HasOne("doneillspa.Models.Timesheet", "Timesheet")
                        .WithMany("TimesheetEntries")
                        .HasForeignKey("TimesheetId");
                });

            modelBuilder.Entity("doneillspa.Models.TimesheetNote", b =>
                {
                    b.HasOne("doneillspa.Models.Timesheet", "Timesheet")
                        .WithMany("TimesheetNotes")
                        .HasForeignKey("TimesheetId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("doneillspa.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("doneillspa.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("doneillspa.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("doneillspa.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("doneillspa.Models.EmailNotification", b =>
                {
                    b.HasOne("doneillspa.Models.ApplicationUser", "User")
                        .WithMany("EmailNotifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
