using LiteAbpUBD.Business;
using LiteAbpUBD.Business.Dtos;
using LiteAbpUBD.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Authorization.Permissions;

namespace LiteAbpUBD.Web.Controllers
{
    [Route("Role")]
    [Authorize(PermissionConsts.角色管理)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RoleController : AbpController
    {
        protected RoleService RoleService { get; }
        protected PermissionValueProvider PermissionValueProvider { get; }
        public RoleController(RoleService roleService)
        {
            RoleService = roleService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (Request.IsAjax())
            {
                var roles = RoleService.GetAll();
                return PartialView("ListPartialView", roles);
            }
            var permissions = PermissionConsts.GetAll();
            return View(permissions);
        }

        [Authorize(PermissionConsts.角色管理_新增)]
        [Route("Create")]
        public IActionResult Create(RoleCreateOrUpdateDto dto) => CreateOrUpdate(dto);

        [Authorize(PermissionConsts.角色管理_编辑)]
        [Route("Update")]
        public IActionResult Update(RoleCreateOrUpdateDto dto) => CreateOrUpdate(dto);

        private IActionResult CreateOrUpdate(RoleCreateOrUpdateDto dto)
        {
            if (!ModelState.IsValid)
                throw new UserFriendlyException("数据验证未通过..");
            var role = RoleService.CreateOrUpdate(dto);
            return Json(role);
        }

        [Authorize(PermissionConsts.角色管理_删除)]
        [Route("Delete")]
        public IActionResult Delete(Guid id)
        {
            RoleService.Delete(id);
            return new OkResult();
        }
    }
}
