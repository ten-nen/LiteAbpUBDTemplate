using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;

namespace LiteAbpUBD.Business
{
    public class LiteAbpUBDPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var group = context.AddGroup("LiteAbpUBD");
            var permissions = PermissionConsts.GetAll();
            foreach (var permission in permissions)
            {
                group.AddPermission(permission.Name);
            }
        }
    }
}
