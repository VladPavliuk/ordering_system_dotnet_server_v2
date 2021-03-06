﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using mvc_auth.Data;
using System;

namespace mvc_auth.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("mvc_auth.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("ImagePath");

                    b.Property<string>("LastName");

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

            modelBuilder.Entity("mvc_auth.Models.Date", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<TimeSpan?>("From");

                    b.Property<string>("Title");

                    b.Property<TimeSpan?>("To");

                    b.HasKey("ID");

                    b.ToTable("Date");
                });

            modelBuilder.Entity("mvc_auth.Models.Order", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime>("EndedAt");

                    b.Property<int?>("Organization_IDID");

                    b.Property<decimal>("Price");

                    b.Property<int?>("Service_IDID");

                    b.Property<DateTime>("StartedAt");

                    b.Property<string>("User_IDId");

                    b.HasKey("ID");

                    b.HasIndex("Organization_IDID");

                    b.HasIndex("Service_IDID");

                    b.HasIndex("User_IDId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("mvc_auth.Models.Organization", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Description");

                    b.Property<string>("ImagePath");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("UserId");

                    b.ToTable("Organization");
                });

            modelBuilder.Entity("mvc_auth.Models.OrganizationDateRelation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int?>("Date_IDID");

                    b.Property<TimeSpan?>("From");

                    b.Property<bool?>("IsDayAndNight");

                    b.Property<bool?>("IsHoliday");

                    b.Property<int?>("Organization_IDID");

                    b.Property<TimeSpan?>("To");

                    b.HasKey("ID");

                    b.HasIndex("Date_IDID");

                    b.HasIndex("Organization_IDID");

                    b.ToTable("OrganizationDateRelation");
                });

            modelBuilder.Entity("mvc_auth.Models.OrganizationRating", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int?>("Order_IDID");

                    b.Property<string>("User_IDId");

                    b.Property<decimal>("Value");

                    b.HasKey("ID");

                    b.HasIndex("Order_IDID");

                    b.HasIndex("User_IDId");

                    b.ToTable("OrganizationMarkup");
                });

            modelBuilder.Entity("mvc_auth.Models.OrganizationServiceRelation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<decimal>("Duration");

                    b.Property<string>("ImagePath");

                    b.Property<int?>("Organization_IDID");

                    b.Property<decimal>("Price");

                    b.Property<int?>("Service_IDID");

                    b.HasKey("ID");

                    b.HasIndex("Organization_IDID");

                    b.HasIndex("Service_IDID");

                    b.ToTable("OrganizationServiceRelation");
                });

            modelBuilder.Entity("mvc_auth.Models.Service", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<decimal>("Duration");

                    b.Property<string>("ImagePath");

                    b.Property<decimal>("Price");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("Service");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("mvc_auth.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("mvc_auth.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("mvc_auth.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("mvc_auth.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("mvc_auth.Models.Order", b =>
                {
                    b.HasOne("mvc_auth.Models.Organization", "Organization_ID")
                        .WithMany()
                        .HasForeignKey("Organization_IDID");

                    b.HasOne("mvc_auth.Models.Service", "Service_ID")
                        .WithMany()
                        .HasForeignKey("Service_IDID");

                    b.HasOne("mvc_auth.Models.ApplicationUser", "User_ID")
                        .WithMany()
                        .HasForeignKey("User_IDId");
                });

            modelBuilder.Entity("mvc_auth.Models.Organization", b =>
                {
                    b.HasOne("mvc_auth.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("mvc_auth.Models.OrganizationDateRelation", b =>
                {
                    b.HasOne("mvc_auth.Models.Date", "Date_ID")
                        .WithMany()
                        .HasForeignKey("Date_IDID");

                    b.HasOne("mvc_auth.Models.Organization", "Organization_ID")
                        .WithMany()
                        .HasForeignKey("Organization_IDID");
                });

            modelBuilder.Entity("mvc_auth.Models.OrganizationRating", b =>
                {
                    b.HasOne("mvc_auth.Models.Order", "Order_ID")
                        .WithMany()
                        .HasForeignKey("Order_IDID");

                    b.HasOne("mvc_auth.Models.ApplicationUser", "User_ID")
                        .WithMany()
                        .HasForeignKey("User_IDId");
                });

            modelBuilder.Entity("mvc_auth.Models.OrganizationServiceRelation", b =>
                {
                    b.HasOne("mvc_auth.Models.Organization", "Organization_ID")
                        .WithMany()
                        .HasForeignKey("Organization_IDID");

                    b.HasOne("mvc_auth.Models.Service", "Service_ID")
                        .WithMany()
                        .HasForeignKey("Service_IDID");
                });
#pragma warning restore 612, 618
        }
    }
}
