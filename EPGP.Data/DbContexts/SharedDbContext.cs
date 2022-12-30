using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EPGP.Data.DbContexts
{
    public class SharedDbContext : DbContext
    {
        public SharedDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();

            optionsBuilder.UseNpgsql(config.GetConnectionString("DefaultConnection"));
        }
    }
}
