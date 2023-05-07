
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LiteAbpUBD.Example.DataAccess
{
    public class ExampleDbContextFactory : IDesignTimeDbContextFactory<ExampleDbContext>
    {
        public ExampleDbContext CreateDbContext(string[] args)
        {

            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<ExampleDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Example"));

            return new ExampleDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../../src/LiteAbpUBD.Web/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
