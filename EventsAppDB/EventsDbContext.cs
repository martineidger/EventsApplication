using EventsAppDB.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventsAppDB
{
    public class EventsDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public EventsDbContext(DbContextOptions options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Participant>().HasData(
               new Participant {Id = 1,  Name = "Tom", Surname = "Hallow", BirthDate = new DateOnly(2005, 10, 2), Email = "tom.hallow@example.com", RegistrationDate = DateTime.Today },
               new Participant {Id = 2,  Name = "John", Surname = "Doe", BirthDate = new DateOnly(1990, 5, 15), Email = "john.doe@example.com", RegistrationDate = DateTime.Today }
                );
        }
    }
}
