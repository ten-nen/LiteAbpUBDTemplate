using LiteAbpUBD.Business.Dtos;
using LiteAbpUBD.DataAccess;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Caching;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;

namespace LiteAbpUBD.Business.Services
{
    public class RoleService : ApplicationService
    {
        protected LiteAbpUBDDbContext DbContext => LazyServiceProvider.LazyGetRequiredService<IDbContextProvider<LiteAbpUBDDbContext>>().GetDbContextAsync().Result;
        protected IPermissionValueProvider PermissionValueProvider { get; }
        protected IDistributedCache<PermissionGrantCacheItem> Cache { get; }

        public RoleService(
            RolePermissionValueProvider rolePermissionValueProvider,
            IDistributedCache<PermissionGrantCacheItem> cache)
        {
            PermissionValueProvider = rolePermissionValueProvider;
            Cache = cache;
        }

        public virtual List<RoleDto> GetAll()
        {
            var roles = DbContext.Set<IdentityRole>();
            var roleNames = roles.Select(x => x.Name);
            var permissions = DbContext.Set<PermissionGrant>().Where(x => x.ProviderName == PermissionValueProvider.Name && roleNames.Contains(x.ProviderKey)).ToList();
            var r = ObjectMapper.Map<List<IdentityRole>, List<RoleDto>>(roles.ToList());
            r.ForEach(x =>
            {
                x.Permissions = permissions.Where(p => p.ProviderKey == x.Name).Select(x => x.Name).ToList();
            });
            return r;
        }

        public virtual RoleDto CreateOrUpdate(RoleCreateOrUpdateDto dto)
        {
            IdentityRole role;
            if (dto.Id.HasValue)
            {
                role = DbContext.Set<IdentityRole>().FirstOrDefault(x => x.Id == dto.Id.Value);
                if (role == null)
                    throw new UserFriendlyException("角色不存在");
                if (role.Name != dto.Name)
                {
                    var exists = DbContext.Set<IdentityRole>().Any(x => x.Id != role.Id && x.Name == dto.Name);
                    if (exists)
                        throw new UserFriendlyException("角色名称已存在");

                    role.ChangeName(dto.Name);
                    DbContext.Set<IdentityRole>().Update(role);
                }
            }
            else
            {
                var exsits = DbContext.Set<IdentityRole>().Any(x => x.Name == dto.Name);
                if (exsits)
                    throw new UserFriendlyException("角色名称已存在");

                role = new IdentityRole(GuidGenerator.Create(), dto.Name);
                DbContext.Set<IdentityRole>().Add(role);
            }

            if (dto.Id.HasValue)
            {
                var deletes = DbContext.Set<PermissionGrant>().Where(x => x.ProviderName == PermissionValueProvider.Name && x.ProviderKey == dto.Name);

                //删除权限缓存
                Cache.RemoveMany(deletes.Select(x => PermissionGrantCacheItem.CalculateCacheKey(x.Name, PermissionValueProvider.Name, dto.Name)));

                DbContext.Set<PermissionGrant>().RemoveRange(deletes);
            }
            if (dto.Permissions != null && dto.Permissions.Any())
                DbContext.Set<PermissionGrant>().AddRange(dto.Permissions.Select(x => new PermissionGrant(GuidGenerator.Create(), x, PermissionValueProvider.Name, role.Name)));

            DbContext.SaveChanges();

            return ObjectMapper.Map<IdentityRole, RoleDto>(role);
        }

        public virtual void Delete(Guid id)
        {
            var role = DbContext.Set<IdentityRole>().FirstOrDefault(x => x.Id == id);
            if (role == null)
                return;

            var rolePermissions = DbContext.Set<PermissionGrant>().Where(x => x.ProviderKey == role.Name);

            //删除权限缓存
            Cache.RemoveMany(rolePermissions.Select(x => PermissionGrantCacheItem.CalculateCacheKey(x.Name, PermissionValueProvider.Name, role.Name)));

            DbContext.Set<PermissionGrant>().RemoveRange(rolePermissions);
            DbContext.Set<IdentityRole>().Remove(role);

            DbContext.SaveChanges();
        }
    }
}
