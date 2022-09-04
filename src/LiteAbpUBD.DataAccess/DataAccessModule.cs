using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Identity;
#if (mysql)
using Volo.Abp.EntityFrameworkCore.MySQL;
#else
using Volo.Abp.EntityFrameworkCore.SqlServer;
#endif

namespace LiteAbpUBD.DataAccess
{
    [DependsOn(
       typeof(AbpIdentityEntityFrameworkCoreModule),
       typeof(AbpPermissionManagementEntityFrameworkCoreModule),
       typeof(AbpSettingManagementEntityFrameworkCoreModule),
#if (mysql)
       typeof(AbpEntityFrameworkCoreMySQLModule),
#else
       typeof(AbpEntityFrameworkCoreSqlServerModule),
#endif
       typeof(AbpAuditLoggingEntityFrameworkCoreModule)
       )]
    public class DataAccessModule : AbpModule
    {
        //todo: 1、创建数据库 dotnet ef migrations add InitialCreate -o ./Migrations
        //                 dotnet ef database update
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<LiteAbpUBDDbContext>(options =>
            {
                /* Remove "includeAllEntities: true" to create
                 * default repositories only for aggregate roots */
                options.AddDefaultRepositories(includeAllEntities: true);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                /* The main point to change your DBMS.
                 * See also demoMigrationsDbContextFactory for EF Core tooling. */
#if (mysql)
                options.UseMySQL();
#else
                options.UseSqlServer();
#endif

            });
        }
    }
}