using LiteAbpUBD.Business;
using LiteAbpUBD.Business.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Linq;
using System.Reflection;
using Volo.Abp;
using Volo.Abp.SettingManagement;

namespace LiteAbpUBD.Web.Controllers
{
    [Authorize(Permissions.Menus.Default)]
    public class MenuController : BaseController
    {
        protected ISettingManager SettingManager { get; }
        protected IActionDescriptorCollectionProvider ActionDescriptorCollectionProvider { get; }
        public MenuController(
            SettingManager settingManager,
            IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            SettingManager = settingManager;
            ActionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        }

        [HttpGet]
        public async Task<List<MenuDto>> GetAsync(int type)
        {
            var menuSettingKey = type == 1 ? "App:Admin.MenuJson" : "App:MenuJson";
            var menus = new List<MenuDto>();
            var menusJson = await SettingManager.GetOrNullGlobalAsync(menuSettingKey);
            if (!string.IsNullOrWhiteSpace(menusJson))
                menus = JsonConvert.DeserializeObject<List<MenuDto>>(menusJson);
            return menus;
        }

        private async Task<bool> IsGrantedAsync(string route)
        {
            if (string.IsNullOrWhiteSpace(route)) return true;
            route = route.Equals("/") ? "/home/index" : route;
            var pageActionDescriptor = ActionDescriptorCollectionProvider.ActionDescriptors.Items.FirstOrDefault(x => (x as PageActionDescriptor)?.ViewEnginePath.Equals(route, StringComparison.OrdinalIgnoreCase) == true) as PageActionDescriptor;
            if (pageActionDescriptor == null) return true;
            var pageType = typeof(WebModule).Assembly.DefinedTypes.Where(x => typeof(PageModel).IsAssignableFrom(x)).FirstOrDefault(x => x.FullName.EndsWith($"Pages{pageActionDescriptor.ViewEnginePath.Replace("/", ".")}Model", StringComparison.OrdinalIgnoreCase));
            if (pageType == null) throw new UserFriendlyException($"{route}页面地址必须使用RazorPages，pageType需使用<Page>Model");
            var allowAnonymous = pageType.GetCustomAttribute<AllowAnonymousAttribute>();
            if (allowAnonymous != null) return true;
            var authorize = pageType.GetCustomAttribute<AuthorizeAttribute>();
            if (authorize == null) return true;
            if (authorize.Policy != null) return await AuthorizationService.IsGrantedAsync(authorize.Policy);
            return CurrentUser.IsAuthenticated;
        }
        [HttpGet("authed")]
        public async Task<List<MenuDto>> GetAuthedListAsync(int type)
        {
            var list = await GetAsync(type);
            list = list.Where(x => IsGrantedAsync(x.Route).GetAwaiter().GetResult()).ToList();
            return list;
        }

        [HttpPost]
        [Authorize(Permissions.Menus.Create)]
        public async Task CreateAsync(int type, MenuDto dto)
        {
            var menuSettingKey = type == 1 ? "App:Admin.MenuJson" : "App:MenuJson";
            var menus = await GetAsync(type);
            dto.Id = menus.Max(x => x.Id) + 1;
            menus.Add(dto);
            await SettingManager.SetGlobalAsync(menuSettingKey, JsonConvert.SerializeObject(menus));
        }

        [HttpPut]
        [Authorize(Permissions.Menus.Update)]
        public async Task UpdateAsync(int type, MenuDto dto)
        {
            var menuSettingKey = type == 1 ? "App:Admin.MenuJson" : "App:MenuJson";
            var menus = await GetAsync(type);
            var menu = menus.FirstOrDefault(x => x.Id == dto.Id);
            if (menu != null)
                menus.Remove(menu);
            menus.Add(dto);
            await SettingManager.SetGlobalAsync(menuSettingKey, JsonConvert.SerializeObject(menus));
        }

        [HttpDelete]
        [Authorize(Permissions.Menus.Delete)]
        public async Task DeleteAsync(int type, int id)
        {
            var menuSettingKey = type == 1 ? "App:Admin.MenuJson" : "App:MenuJson";
            var menus = await GetAsync(type);
            menus = menus.Where(x => x.Id != id && x.Pid != id).ToList();
            await SettingManager.SetGlobalAsync(menuSettingKey, JsonConvert.SerializeObject(menus));
        }
    }
}
