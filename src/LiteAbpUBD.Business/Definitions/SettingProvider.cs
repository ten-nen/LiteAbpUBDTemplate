

using Newtonsoft.Json;
using Volo.Abp.Settings;
using LiteAbpUBD.Business.Dtos;

namespace LiteAbpUBD.Business.Definitions
{
    public class SettingProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            var menus = new List<MenuDto>()
            {
                new MenuDto { Id = 1, Pid = 0, Title = "主页", Icon = "home", Route = "/admin/index", Order=10 },
                new MenuDto { Id = 2, Pid = 0, Title = "系统管理", Icon = "user", Route = "", Order = 9 },
                new MenuDto { Id = 3, Pid = 2, Title = "用户管理", Route = "/admin/user", Order = 10 },
                new MenuDto { Id = 4, Pid = 2, Title = "角色管理", Route = "/admin/role", Order = 9 },
                new MenuDto { Id = 5, Pid = 2, Title = "菜单管理", Route = "/admin/menu", Order = 8},
                new MenuDto { Id = 6, Pid = 2, Title = "后台菜单管理", Route = "/admin/menu?type=1", Order = 7},
            };
            context.Add(
                new SettingDefinition("App:Admin.MenuJson", JsonConvert.SerializeObject(menus))
            );

            menus = new List<MenuDto>()
            {
                new MenuDto { Id = 1, Pid = 0, Title = "主页", Icon = "home", Route = "/home/index", Order=10 },
            };
            context.Add(
                new SettingDefinition("App:MenuJson", JsonConvert.SerializeObject(menus))
            );
        }
    }
}
