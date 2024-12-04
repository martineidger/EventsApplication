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
                new Event { Id = 1, Address = "adr1", Category = EventCategory.Concert, Description = "desc", EventTime = new DateTime(2025, 12,12), MaxLimit = 20, Name = "name" },
                new Event { Id = 2, Address = "adr2", Category = EventCategory.Lecture, Description = "desc2", EventTime = new DateTime(2012, 12, 12), MaxLimit = 30, Name = "name2" },
                new Event { Id = 3, Address = "adr3", Category = EventCategory.Exhibition, Description = "desc3", EventTime = new DateTime(2024, 12, 12), MaxLimit = 40, Name = "name3" }
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
