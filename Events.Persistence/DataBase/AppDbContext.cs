using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Events.Domain.Entities;
//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Events.Persistence.DataBase
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(); 

            /*optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));*/
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasMany(u => u.Events)
            .WithMany(e => e.Users)
            .UsingEntity(en => en.ToTable("EventUser"));

            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Event>(ent =>
                {
                    ent.HasKey(e => e.Id);
                    ent.Property(e => e.Id).ValueGeneratedOnAdd();
                }
            );
            modelBuilder.Entity<User>(ent =>
                {
                    ent.HasKey(e => e.Id);
                    ent.Property(e => e.Id).ValueGeneratedOnAdd();
                }
            );

            modelBuilder.Entity<Event>().HasData(
                new Event { Id = 1, Address = "123 Elm Street, Springfield, IL 62704, USA", Category = EventCategory.Concert, Description = "Join our workshop to learn about modern digital marketing strategies. Get hands-on experience with practical tasks and expert tips.", EventTime = new DateTime(2025, 12,12), MaxLimit = 20, Name = "Digital Marketing Workshop" },
                new Event { Id = 2, Address = "456 Maple Avenue, Los Angeles, CA 90001, USA", Category = EventCategory.Lecture, Description = "Don’t miss a live performance by a local rock band at a cozy bar! Great atmosphere and fantastic music guaranteed.", EventTime = new DateTime(2012, 12, 12), MaxLimit = 30, Name = " Local Band Concert" },
                new Event { Id = 3, Address = "789 Oak Lane, New York, NY 10001, USA", Category = EventCategory.Exhibition, Description = "Come to the street food festival and enjoy a variety of dishes from the best local chefs. Great food and live music await you!", EventTime = new DateTime(2024, 12, 12), MaxLimit = 40, Name = "Street Food Festival" }
            );

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Admin", Surname = "Admin", BirthDate = new DateOnly(1999, 12, 12), Email = "admin1@mail.com", Password = "Admin123!", Role = "Admin",  RegistrationDate = new DateTime(2024, 12, 1) },
                new User { Id = 2, Name = "User", Surname = "User", BirthDate = new DateOnly(1999, 12, 12), Email = "user1@mail.com", Password = "User12345!", Role = "User", RegistrationDate = new DateTime(2024, 12, 1) }
                );
        }

        #region Transactions
        public void BeginTransaction()
        {
            Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            Database.CommitTransaction();
        }
        public async Task CommitAsync()
        {
            await Database.CommitTransactionAsync();
        }

        public void RollbackTransaction()
        {
            Database.RollbackTransaction();
        }

        public async Task RollbackTransactionAsync()
        {
            await Database.RollbackTransactionAsync();
        }
        #endregion
    }
}
