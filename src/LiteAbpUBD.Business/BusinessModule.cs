using LiteAbpUBD.DataAccess;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.AuditLogging;
using Volo.Abp.Application;
using Volo.Abp.Localization;
using Volo.Abp.VirtualFileSystem;
using LiteAbpUBD.Business.Localization;
using LiteAbpUBD.Common;

namespace LiteAbpUBD.Business
{
    [DependsOn(
        typeof(CommonModule),
        typeof(DataAccessModule),
        typeof(AbpLocalizationModule),
        typeof(AbpDddApplicationModule),
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

            //本地化
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<BusinessModule>();
            });
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<BusinessResource>("zh-Hans")
                    .AddVirtualJson("/Localization/BusinessResources");
            });

            
        }

        public async override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
        {

        }
    }
}