using LiteAbpUBD.Business.Dtos;
using LiteAbpUBD.DataAccess;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.PermissionManagement;

namespace LiteAbpUBD.Business.Services
{
    public class PermissionService : ApplicationService
    {
        protected LiteAbpUBDDbContext DbContext => LazyServiceProvider.LazyGetRequiredService<IDbContextProvider<LiteAbpUBDDbContext>>().GetDbContextAsync().Result;
        protected IPermissionValueProvider PermissionValueProvider { get; }
        public PermissionService(
            RolePermissionValueProvider rolePermissionValueProvider)
        {
            PermissionValueProvider = rolePermissionValueProvider;
        }

        public virtual List<PermissionDto> GetListByKeys(IEnumerable<string> keys)
        {
            var permissions = DbContext.Set<PermissionGrant>().Where(x => x.ProviderName == PermissionValueProvider.Name && keys.Contains(x.ProviderKey)).ToList();
            return ObjectMapper.Map<List<PermissionGrant>, List<PermissionDto>>(permissions);
        }
    }
}
