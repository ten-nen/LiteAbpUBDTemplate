using LiteAbpUBD.Business;
using LiteAbpUBD.Business.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.SettingManagement;

namespace LiteAbpUBD.Web.Controllers
{
    [Route("Menu")]
    [Authorize(PermissionConsts.菜单管理)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class MenuController : AbpController
    {
        protected ISettingManager SettingManager { get; }
        protected const string MenuSettingKey = "Site.MenuJson";
        public MenuController(SettingManager settingManager)
        {
            SettingManager = settingManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (Request.IsAjax())
            {
                var menus = await GetMenusAsync();
                return PartialView("ListPartialView", menus);
            }
            return View();
        }

        private async Task<List<MenuDto>> GetMenusAsync()
        {
            var menus = new List<MenuDto>();
            var menusJson = await SettingManager.GetOrNullGlobalAsync(MenuSettingKey);
            if (!string.IsNullOrWhiteSpace(menusJson))
                menus = JsonConvert.DeserializeObject<List<MenuDto>>(menusJson);
            return menus;
        }

        [Route("Create")]
        [Authorize(PermissionConsts.菜单管理_新增)]
        public async Task<IActionResult> Create(MenuDto dto)
        {
            var menus = await GetMenusAsync();
            dto.Id = menus.Max(x => x.Id) + 1;
            menus.Add(dto);
            await SettingManager.SetGlobalAsync(MenuSettingKey, JsonConvert.SerializeObject(menus));
            return Json(dto);
        }

        [Route("Update")]
        [Authorize(PermissionConsts.菜单管理_编辑)]
        public async Task<IActionResult> Update(MenuDto dto)
        {
            var menus = await GetMenusAsync();
            var menu = menus.FirstOrDefault(x => x.Id == dto.Id);
            if (menu != null)
                menus.Remove(menu);
            menus.Add(dto);
            await SettingManager.SetGlobalAsync(MenuSettingKey, JsonConvert.SerializeObject(menus));
            return Json(dto);
        }

        [Authorize(PermissionConsts.菜单管理_删除)]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var menus = await GetMenusAsync();
            menus = menus.Where(x => x.Id != id && x.Pid != id).ToList();
            await SettingManager.SetGlobalAsync(MenuSettingKey, JsonConvert.SerializeObject(menus));
            return Json(id);
        }
    }
}
