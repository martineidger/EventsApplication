using Events.Identity.DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace Events.Identity.DataBase.Context
{
    public class IdentityDataBaseContext : DbContext
    {
        public DbSet<IdentityUser> Users;
        public DbSet<IdentityEvent> Events;
        public IdentityDataBaseContext(DbContextOptions<IdentityDataBaseContext> options) : base(options)
        {
        }
    }
}
