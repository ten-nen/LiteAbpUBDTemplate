using LiteAbpUBD.Business.Dtos;
using System.Reflection;

namespace LiteAbpUBD.Business
{
    public static class PermissionConsts
    {
        public const string 角色管理 = "Role";
        public const string 角色管理_新增 = "Role.Create";
        public const string 角色管理_编辑 = "Role.Update";
        public const string 角色管理_删除 = "Role.Delete";

        public const string 用户管理 = "User";
        public const string 用户管理_新增 = "User.Create";
        public const string 用户管理_编辑 = "User.Update";
        public const string 用户管理_删除 = "User.Delete";
        public static List<PermissionDto> GetAll()
        {
            return typeof(PermissionConsts).GetFields(BindingFlags.Static | BindingFlags.Public).Where(x => x.IsLiteral && !x.IsInitOnly).Select(x => new PermissionDto() { Name = x.GetValue(null)!.ToString(), DisplayName = x.Name }).ToList();
        }
    }
}
