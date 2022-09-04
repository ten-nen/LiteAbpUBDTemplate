
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LiteAbpUBD.DataAccess
{
    public class LiteAbpUBDDbContextFactory : IDesignTimeDbContextFactory<LiteAbpUBDDbContext>
    {
        public LiteAbpUBDDbContext CreateDbContext(string[] args)
        {

            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<LiteAbpUBDDbContext>()
#if (mysql)
                .UseMySql(configuration.GetConnectionString("Default"), MySqlServerVersion.LatestSupportedServerVersion);
#else
                .UseSqlServer(configuration.GetConnectionString("Default"));
#endif

            return new LiteAbpUBDDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../LiteAbpUBD.Web/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
