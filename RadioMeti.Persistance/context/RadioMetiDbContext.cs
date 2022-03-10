using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RadioMeti.Domain.Entities.Log;

namespace RadioMeti.Persistance.context
{
    public class RadioMetiDbContext:IdentityDbContext
    {
        public RadioMetiDbContext(DbContextOptions<RadioMetiDbContext> options) : base(options)
        {
        }
        public DbSet<ErrorLog> ErrorLogs { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
