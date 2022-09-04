using LiteAbpUBD.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.AutoMapper;
using Volo.Abp.AuditLogging;

namespace LiteAbpUBD.Business
{
    [DependsOn(
        typeof(Volo.Abp.Application.AbpDddApplicationModule),
        typeof(DataAccessModule),
        typeof(AbpAuditLoggingDomainModule),
        typeof(AbpIdentityDomainModule),
        typeof(AbpPermissionManagementDomainModule),
        typeof(AbpSettingManagementDomainModule)
        )]
    public class BusinessModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //配置AutoMapper
            Configure<AbpAutoMapperOptions>(options => options.AddMaps<BusinessModule>());
        }

        public async override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            await SeedDataAsync(context);
        }

        private async Task SeedDataAsync(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                await scope.ServiceProvider
                    .GetRequiredService<IDataSeeder>()
                    .SeedAsync(new DataSeedContext(null)
                         .WithProperty(IdentityDataSeedContributor.AdminEmailPropertyName, IdentityDataSeedContributor.AdminEmailDefaultValue)
                         .WithProperty(IdentityDataSeedContributor.AdminPasswordPropertyName, "123456"));
            }
        }
    }
}