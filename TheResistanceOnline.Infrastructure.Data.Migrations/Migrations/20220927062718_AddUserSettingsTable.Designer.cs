﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TheResistanceOnline.Data;
using TheResistanceOnline.Infrastructure.Data;

#nullable disable

namespace TheResistanceOnline.Infrastructure.Data.Migrations.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20220927062718_AddUserSettingsTable")]
    partial class AddUserSettingsTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("TheResistanceOnline.Data.DiscordServer.DiscordChannel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id")
                        .HasName("PK_DiscordChannels");

                    b.ToTable("DiscordChannels");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "game-1"
                        },
                        new
                        {
                            Id = 2,
                            Name = "game-2"
                        },
                        new
                        {
                            Id = 3,
                            Name = "game-3"
                        },
                        new
                        {
                            Id = 4,
                            Name = "game-4"
                        },
                        new
                        {
                            Id = 5,
                            Name = "game-5"
                        },
                        new
                        {
                            Id = 6,
                            Name = "game-6"
                        },
                        new
                        {
                            Id = 7,
                            Name = "game-7"
                        },
                        new
                        {
                            Id = 8,
                            Name = "game-8"
                        },
                        new
                        {
                            Id = 9,
                            Name = "game-9"
                        },
                        new
                        {
                            Id = 10,
                            Name = "game-10"
                        });
                });

            modelBuilder.Entity("TheResistanceOnline.Data.DiscordServer.DiscordRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("DiscordChannelId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id")
                        .HasName("PK_DiscordRoles");

                    b.HasIndex("DiscordChannelId");

                    b.ToTable("DiscordRoles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DiscordChannelId = 1,
                            Name = "Can Join Game-1"
                        },
                        new
                        {
                            Id = 2,
                            DiscordChannelId = 2,
                            Name = "Can Join Game-2"
                        },
                        new
                        {
                            Id = 3,
                            DiscordChannelId = 3,
                            Name = "Can Join Game-3"
                        },
                        new
                        {
                            Id = 4,
                            DiscordChannelId = 4,
                            Name = "Can Join Game-4"
                        },
                        new
                        {
                            Id = 5,
                            DiscordChannelId = 5,
                            Name = "Can Join Game-5"
                        },
                        new
                        {
                            Id = 6,
                            DiscordChannelId = 6,
                            Name = "Can Join Game-6"
                        },
                        new
                        {
                            Id = 7,
                            DiscordChannelId = 7,
                            Name = "Can Join Game-7"
                        },
                        new
                        {
                            Id = 8,
                            DiscordChannelId = 8,
                            Name = "Can Join Game-8"
                        },
                        new
                        {
                            Id = 9,
                            DiscordChannelId = 9,
                            Name = "Can Join Game-9"
                        },
                        new
                        {
                            Id = 10,
                            DiscordChannelId = 10,
                            Name = "Can Join Game-10"
                        });
                });

            modelBuilder.Entity("TheResistanceOnline.Data.DiscordServer.DiscordUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("DiscordRoleId")
                        .HasColumnType("int");

                    b.Property<string>("DiscordTag")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id")
                        .HasName("PK_DiscordUsers");

                    b.HasIndex("DiscordRoleId");

                    b.ToTable("DiscordUsers");
                });

            modelBuilder.Entity("TheResistanceOnline.Data.ProfilePictures.ProfilePicture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)")
                        .HasColumnName("Name");

                    b.HasKey("Id")
                        .HasName("PK_ProfilePictures");

                    b.ToTable("ProfilePictures");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Black man cyberpunk",
                            Name = "1.png"
                        },
                        new
                        {
                            Id = 2,
                            Description = "White woman cyberpunk",
                            Name = "2.png"
                        },
                        new
                        {
                            Id = 3,
                            Description = "White man spy",
                            Name = "3.png"
                        },
                        new
                        {
                            Id = 4,
                            Description = "White woman spy",
                            Name = "4.png"
                        },
                        new
                        {
                            Id = 5,
                            Description = "White man cyber warrior",
                            Name = "5.png"
                        });
                });

            modelBuilder.Entity("TheResistanceOnline.Data.Users.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DiscordUserId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<int?>("ProfilePictureId")
                        .HasColumnType("int");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int>("UserSettingId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DiscordUserId");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("ProfilePictureId");

                    b.HasIndex("UserSettingId");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("TheResistanceOnline.Data.UserSettings.UserSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("UserWantsToUseDiscord")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset>("UserWantsToUseDiscordRecord")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("UserSettings");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TheResistanceOnline.Data.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TheResistanceOnline.Data.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TheResistanceOnline.Data.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("TheResistanceOnline.Data.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TheResistanceOnline.Data.DiscordServer.DiscordRole", b =>
                {
                    b.HasOne("TheResistanceOnline.Data.DiscordServer.DiscordChannel", "DiscordChannel")
                        .WithMany()
                        .HasForeignKey("DiscordChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DiscordChannel");
                });

            modelBuilder.Entity("TheResistanceOnline.Data.DiscordServer.DiscordUser", b =>
                {
                    b.HasOne("TheResistanceOnline.Data.DiscordServer.DiscordRole", "DiscordRole")
                        .WithMany()
                        .HasForeignKey("DiscordRoleId");

                    b.Navigation("DiscordRole");
                });

            modelBuilder.Entity("TheResistanceOnline.Data.Users.User", b =>
                {
                    b.HasOne("TheResistanceOnline.Data.DiscordServer.DiscordUser", "DiscordUser")
                        .WithMany()
                        .HasForeignKey("DiscordUserId");

                    b.HasOne("TheResistanceOnline.Data.ProfilePictures.ProfilePicture", "ProfilePicture")
                        .WithMany()
                        .HasForeignKey("ProfilePictureId");

                    b.HasOne("TheResistanceOnline.Data.UserSettings.UserSetting", "UserSetting")
                        .WithMany()
                        .HasForeignKey("UserSettingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DiscordUser");

                    b.Navigation("ProfilePicture");

                    b.Navigation("UserSetting");
                });
#pragma warning restore 612, 618
        }
    }
}
