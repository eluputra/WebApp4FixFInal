using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LuckyPaw.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LuckyPaw.Data
{
    public class LuckyPawContext : DbContext
    {

        public DbSet<LuckyPaw.Models.PricingPuppyModel> PricingPuppyModel { get; set; }

        public DbSet<LuckyPaw.Models.TrainingDogModel> TrainingDogModel { get; set; }

        public DbSet<LuckyPaw.Models.TrainingServicesPriceModel> TrainingServicesPriceModel { get; set; }

        public DbSet<LuckyPaw.Models.TrainersModel> TrainersModel { get; set; }

        public DbSet<LuckyPaw.Models.CartItemModel> CartItemModel { get; set; }

        public DbSet<Microsoft.AspNetCore.Identity.IdentityRole> IdentityRole { get; set; }

        public DbSet<Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>> IdentityRoleClaim { get; set; }

        public DbSet<Microsoft.AspNetCore.Identity.IdentityUser> IdentityUser { get; set; }

        public DbSet<Microsoft.AspNetCore.Identity.IdentityUserClaim<string>> IdentityUserClaim { get; set; }

        public DbSet<Microsoft.AspNetCore.Identity.IdentityUserLogin<string>> IdentityUserLogin { get; set; }

        public DbSet<Microsoft.AspNetCore.Identity.IdentityUserRole<string>> IdentityUserRole { get; set; }

        public DbSet<Microsoft.AspNetCore.Identity.IdentityUserToken<string>> IdentityUserToken { get; set; }

        public LuckyPawContext(DbContextOptions<LuckyPawContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) // this code for building the model 
        {
            modelBuilder.Entity<PricingPuppyModel>().ToTable("PricePuppy");
            modelBuilder.Entity<TrainingDogModel>().ToTable("TrainingDog");
            modelBuilder.Entity<TrainingServicesPriceModel>().ToTable("TrainingServicesPrice");
            modelBuilder.Entity<TrainersModel>().ToTable("Trainers");
            modelBuilder.Entity<CartItemModel>().ToTable("CartItem");

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
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("ClaimType");

                b.Property<string>("ClaimValue");

                b.Property<string>("RoleId")
                    .IsRequired();

                b.HasKey("Id");

                b.HasIndex("RoleId");

                b.ToTable("AspNetRoleClaims");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
            {
                b.Property<string>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<int>("AccessFailedCount");

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken();

                b.Property<string>("Email")
                    .HasMaxLength(256);

                b.Property<bool>("EmailConfirmed");

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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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
                b.Property<string>("LoginProvider")
                    .HasMaxLength(128);

                b.Property<string>("ProviderKey")
                    .HasMaxLength(128);

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

                b.Property<string>("LoginProvider")
                    .HasMaxLength(128);

                b.Property<string>("Name")
                    .HasMaxLength(128);

                b.Property<string>("Value");

                b.HasKey("UserId", "LoginProvider", "Name");

                b.ToTable("AspNetUserTokens");
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
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
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

                b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

    }
}
