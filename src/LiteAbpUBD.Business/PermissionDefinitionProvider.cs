
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using LiteAbpUBD.Business.Localization;

namespace LiteAbpUBD.Business
{
    public class PermissionDefinitionProvider : Volo.Abp.Authorization.Permissions.PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var permissionNames = Permissions.GetAll();
            foreach (var groupPermissionName in permissionNames.Where(x => !x.Contains(".")))
            {
                var group = context.AddGroup(groupPermissionName, LocalizableString.Create<BusinessResource>($"Permission:{groupPermissionName}"));
                foreach (var permissionName in permissionNames.Where(x => x.StartsWith(groupPermissionName + ".") && x.Split('.').Length == 2))
                {
                    var permission = group.AddPermission(permissionName, LocalizableString.Create<BusinessResource>($"Permission:{permissionName}"));
                    foreach (var childPermissionName in permissionNames.Where(x => x != permissionName && x.StartsWith(permissionName + ".") ))
                    {
                        permission.AddChild(childPermissionName, LocalizableString.Create<BusinessResource>($"Permission:{childPermissionName}"));
                    }
                }
            }
        }
    }
}
