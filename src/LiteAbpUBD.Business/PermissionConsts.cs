using LiteAbpUBD.Business.Dtos;
using System.Reflection;

namespace LiteAbpUBD.Business
{
    public static class PermissionConsts
    {
        public const string 主页 = "home";

        public const string 角色管理 = "role";
        public const string 角色管理_新增 = "role.create";
        public const string 角色管理_编辑 = "role.update";
        public const string 角色管理_删除 = "role.delete";

        public const string 用户管理 = "user";
        public const string 用户管理_新增 = "user.create";
        public const string 用户管理_编辑 = "user.update";
        public const string 用户管理_删除 = "user.delete";

        public const string 菜单管理 = "menu";
        public const string 菜单管理_新增 = "menu.create";
        public const string 菜单管理_编辑 = "menu.update";
        public const string 菜单管理_删除 = "menu.delete";

        #region OpenApis
        public const string 开放接口 = "openapi";
        public const string 开放接口_新增订单 = "openapi.createorder";
        #endregion
        public static List<PermissionDto> GetAll()
        {
            return typeof(PermissionConsts).GetFields(BindingFlags.Static | BindingFlags.Public).Where(x => x.IsLiteral && !x.IsInitOnly).Select(x => new PermissionDto() { Name = x.GetValue(null)!.ToString(), DisplayName = x.Name }).ToList();
        }
    }
}
