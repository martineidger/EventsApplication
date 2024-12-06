﻿// <auto-generated />
using System;
using Events.Persistence.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Events.Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-rc.2.24474.1")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EventUser", b =>
                {
                    b.Property<int>("EventsId")
                        .HasColumnType("int");

                    b.Property<int>("UsersId")
                        .HasColumnType("int");

                    b.HasKey("EventsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("EventUser", (string)null);
                });

            modelBuilder.Entity("Events.Domain.Entities.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Category")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EventTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MaxLimit")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Events");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "123 Elm Street, Springfield, IL 62704, USA",
                            Category = 2,
                            Description = "Join our workshop to learn about modern digital marketing strategies. Get hands-on experience with practical tasks and expert tips.",
                            EventTime = new DateTime(2025, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            MaxLimit = 20,
                            Name = "Digital Marketing Workshop"
                        },
                        new
                        {
                            Id = 2,
                            Address = "456 Maple Avenue, Los Angeles, CA 90001, USA",
                            Category = 4,
                            Description = "Don’t miss a live performance by a local rock band at a cozy bar! Great atmosphere and fantastic music guaranteed.",
                            EventTime = new DateTime(2012, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            MaxLimit = 30,
                            Name = " Local Band Concert"
                        },
                        new
                        {
                            Id = 3,
                            Address = "789 Oak Lane, New York, NY 10001, USA",
                            Category = 3,
                            Description = "Come to the street food festival and enjoy a variety of dishes from the best local chefs. Great food and live music await you!",
                            EventTime = new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            MaxLimit = 40,
                            Name = "Street Food Festival"
                        });
                });

            modelBuilder.Entity("Events.Domain.Entities.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notification");
                });

            modelBuilder.Entity("Events.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("BirthDate")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BirthDate = new DateOnly(1999, 12, 12),
                            Email = "admin1@mail.com",
                            Name = "Admin",
                            Password = "Admin123!",
                            RegistrationDate = new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Role = "Admin",
                            Surname = "Admin"
                        },
                        new
                        {
                            Id = 2,
                            BirthDate = new DateOnly(1999, 12, 12),
                            Email = "user1@mail.com",
                            Name = "User",
                            Password = "User12345!",
                            RegistrationDate = new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Role = "User",
                            Surname = "User"
                        });
                });

            modelBuilder.Entity("EventUser", b =>
                {
                    b.HasOne("Events.Domain.Entities.Event", null)
                        .WithMany()
                        .HasForeignKey("EventsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Events.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Events.Domain.Entities.Notification", b =>
                {
                    b.HasOne("Events.Domain.Entities.User", null)
                        .WithMany("Notifications")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Events.Domain.Entities.User", b =>
                {
                    b.Navigation("Notifications");
                });
#pragma warning restore 612, 618
        }
    }
}
