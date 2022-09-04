using LiteAbpUBD.Business;
using LiteAbpUBD.Business.Dtos;
using LiteAbpUBD.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LiteAbpUBD.Web.Controllers
{
    [Authorize(PermissionConsts.用户管理)]
    [Route("User")]
    public class UserController : AbpController
    {
        protected UserService UserService { get; }
        protected RoleService RoleService { get; }
        public UserController(
            UserService userService,
            RoleService roleService)
        {
            UserService = userService;
            RoleService = roleService;
        }
        public IActionResult Index(UserPagerQueryDto dto)
        {
            if (Request.IsAjax())
            {
                var users = UserService.GetList(dto);
                return PartialView("ListPartialView", users);
            }
            var roles = RoleService.GetAll();
            return View(roles);
        }


        [Authorize(PermissionConsts.用户管理_新增)]
        [Route("Create")]
        public IActionResult Create(UserCreateOrUpdateDto dto)=>CreateOrUpdate(dto);

        [Authorize(PermissionConsts.用户管理_编辑)]
        [Route("Update")]
        public IActionResult Update(UserCreateOrUpdateDto dto) => CreateOrUpdate(dto);

        private IActionResult CreateOrUpdate(UserCreateOrUpdateDto dto)
        {
            if (!ModelState.IsValid)
                throw new UserFriendlyException("数据验证未通过..");
            var role = UserService.CreateOrUpdate(dto);
            return Json(role);
        }

        [Authorize(PermissionConsts.用户管理_删除)]
        [Route("Delete")]
        public IActionResult Delete(Guid id)
        {
            UserService.Delete(id);
            return new OkResult();
        }
    }
}
