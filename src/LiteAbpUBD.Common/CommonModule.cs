
using Volo.Abp.Modularity;
using Volo.Abp.Validation;

namespace LiteAbpUBD.Common
{
    [DependsOn(
        typeof(AbpValidationModule)
    )]
    public class CommonModule : AbpModule
    {
    }
}
