

using Newtonsoft.Json;
using Volo.Abp.Settings;
using LiteAbpUBD.Business.Dtos;

namespace LiteAbpUBD.Business.Definitions
{
    public class SiteSettingProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            var menus = new List<MenuDto>()
            {
                new MenuDto { Id = 1, Pid = 0, Title = "主页", Icon = "home", Route = "/home", Order=10 },
                new MenuDto { Id = 2, Pid = 0, Title = "系统管理", Icon = "user", Route = "", Order = 9 },
                new MenuDto { Id = 3, Pid = 2, Title = "用户管理", Route = "/user", Order = 10 },
                new MenuDto { Id = 4, Pid = 2, Title = "角色管理", Route = "/role", Order = 9 },
                new MenuDto { Id = 5, Pid = 2, Title = "菜单管理", Route = "/menu", Order = 8}
            };
            context.Add(
                new SettingDefinition("Site.MenuJson", JsonConvert.SerializeObject(menus))
            );
        }
    }
}
