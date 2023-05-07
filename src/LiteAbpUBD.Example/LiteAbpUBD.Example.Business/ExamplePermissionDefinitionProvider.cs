
using LiteAbpUBD.Business.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LiteAbpUBD.Example.Business
{
    public class ExamplePermissionDefinitionProvider : Volo.Abp.Authorization.Permissions.PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            //Example
            var permissionNames = ExamplePermissions.GetAll();
            foreach (var groupPermissionName in permissionNames.Where(x => !x.Contains(".")))
            {
                var group = context.AddGroup(groupPermissionName, LocalizableString.Create<ExampleBusinessResource>($"Permission:{groupPermissionName}"));
                foreach (var permissionName in permissionNames.Where(x => x.StartsWith(groupPermissionName + ".") && x.Split('.').Length == 2))
                {
                    var permission = group.AddPermission(permissionName, LocalizableString.Create<ExampleBusinessResource>($"Permission:{permissionName}"));
                    foreach (var childPermissionName in permissionNames.Where(x => x != permissionName && x.StartsWith(permissionName + ".")))
                    {
                        permission.AddChild(childPermissionName, LocalizableString.Create<ExampleBusinessResource>($"Permission:{childPermissionName}"));
                    }
                }
            }
        }
    }
}
