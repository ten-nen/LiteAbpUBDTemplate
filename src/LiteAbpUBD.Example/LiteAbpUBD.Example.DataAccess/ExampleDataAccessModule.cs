using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.EntityFrameworkCore.SqlServer;

namespace LiteAbpUBD.Example.DataAccess
{
    [DependsOn(
       typeof(AbpEntityFrameworkCoreSqlServerModule)
       )]
    public class ExampleDataAccessModule : AbpModule
    {
        //todo: 1、创建数据库 dotnet ef migrations add InitialCreate --context ExampleDbContext -o ./Migrations
        //                 dotnet ef database update --context ExampleDbContext
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<ExampleDbContext>(options =>
            {
                /* Remove "includeAllEntities: true" to create
                 * default repositories only for aggregate roots */
                options.AddDefaultRepositories(includeAllEntities: true);
            }).AddAbpDbContext<NoneModelBuilderDbContext>(options =>
            {
                options.AddDefaultRepositories(includeAllEntities: true);
            }); ;

            Configure<AbpDbContextOptions>(options =>
            {
                /* The main point to change your DBMS.
                 * See also demoMigrationsDbContextFactory for EF Core tooling. */
                options.UseSqlServer();

            });
        }
    }
}