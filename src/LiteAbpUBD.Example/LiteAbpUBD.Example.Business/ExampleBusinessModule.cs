using LiteAbpUBD.Example.DataAccess;
using Volo.Abp.Modularity;
using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.Application;
using Volo.Abp.BackgroundWorkers.Quartz;
using LiteAbpUBD.Common;
using Volo.Abp.Localization;
using Volo.Abp.VirtualFileSystem;
using LiteAbpUBD.Business.Localization;

namespace LiteAbpUBD.Example.Business
{
    [DependsOn(
        typeof(CommonModule),
        typeof(AbpBackgroundWorkersQuartzModule),
        typeof(AbpDddApplicationModule),
        typeof(ExampleDataAccessModule)
        )]
    public class ExampleBusinessModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //配置AutoMapper
            Configure<AbpAutoMapperOptions>(options => options.AddMaps<ExampleBusinessModule>());

            //本地化
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<ExampleBusinessModule>();
            });
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<ExampleBusinessResource>("zh-Hans")
                    .AddVirtualJson("/Localization/ExampleBusinessResources");
            });
        }

        public async override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
        {

        }
    }
}