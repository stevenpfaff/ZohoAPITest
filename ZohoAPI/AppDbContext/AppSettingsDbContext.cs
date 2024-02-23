using Microsoft.EntityFrameworkCore;
using ZohoAPI.Models;

namespace ZohoAPI.AppDbContext
{
    public class AppSettingsDbContext : DbContext
    {
        public DbSet<Trucks> Trucks { get; private set; }
        public AppSettingsDbContext() : base()
        {

        }
        public AppSettingsDbContext(DbContextOptions<DbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Trucks>().HasKey(s => s.Id);
            builder.Entity<Trucks>().ToTable("Trucks");
        }
    }
}
