using LiteAbpUBD.Business.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Caching;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;

namespace LiteAbpUBD.Business.Services
{
    public class RoleService : BaseService
    {
        protected IPermissionValueProvider PermissionValueProvider { get; }
        protected IDistributedCache<PermissionGrantCacheItem> Cache { get; }

        public RoleService(
            RolePermissionValueProvider rolePermissionValueProvider,
            IDistributedCache<PermissionGrantCacheItem> cache)
        {
            PermissionValueProvider = rolePermissionValueProvider;
            Cache = cache;
        }

        public virtual async Task<List<RoleDto>> GetListAsync(RolePagerQueryDto dto)
        {
            var db = await GetDbContextAsync();
            var roles = db.Set<IdentityRole>().WhereIf(!string.IsNullOrWhiteSpace(dto.Filter), x => x.Name.Contains(dto.Filter));
            var roleNames = roles.Select(x => x.Name);
            var permissions = await db.Set<PermissionGrant>().Where(x => x.ProviderName == PermissionValueProvider.Name && roleNames.Contains(x.ProviderKey)).ToListAsync();
            var r = ObjectMapper.Map<List<IdentityRole>, List<RoleDto>>(roles.ToList());
            r.ForEach(x =>
            {
                x.Permissions = permissions.Where(p => p.ProviderKey == x.Name).Select(x => x.Name).ToList();
            });
            return r;
        }

        public virtual async Task<RoleDto> CreateOrUpdateAsync(RoleCreateOrUpdateDto dto)
        {
            var db = await GetDbContextAsync();
            IdentityRole role;
            if (dto.Id.HasValue)
            {
                role = await db.Set<IdentityRole>().FirstOrDefaultAsync(x => x.Id == dto.Id.Value);
                if (role == null)
                    throw new UserFriendlyException("角色不存在");
                if (role.Name != dto.Name)
                {
                    var exists = await db.Set<IdentityRole>().AnyAsync(x => x.Id != role.Id && x.Name == dto.Name);
                    if (exists)
                        throw new UserFriendlyException("角色名称已存在");

                    role.ChangeName(dto.Name);
                    db.Set<IdentityRole>().Update(role);
                }
            }
            else
            {
                var exsits = await db.Set<IdentityRole>().AnyAsync(x => x.Name == dto.Name);
                if (exsits)
                    throw new UserFriendlyException("角色名称已存在");

                role = new IdentityRole(GuidGenerator.Create(), dto.Name);
                await db.Set<IdentityRole>().AddAsync(role);
            }

            if (dto.Id.HasValue)
            {
                var deletes = db.Set<PermissionGrant>().Where(x => x.ProviderName == PermissionValueProvider.Name && x.ProviderKey == dto.Name);

                //删除权限缓存
                await Cache.RemoveManyAsync(deletes.Select(x => PermissionGrantCacheItem.CalculateCacheKey(x.Name, PermissionValueProvider.Name, dto.Name)));

                db.Set<PermissionGrant>().RemoveRange(deletes);
            }
            if (dto.Permissions != null && dto.Permissions.Any())
                await db.Set<PermissionGrant>().AddRangeAsync(dto.Permissions.Select(x => new PermissionGrant(GuidGenerator.Create(), x, PermissionValueProvider.Name, role.Name)));

            await db.SaveChangesAsync();

            return ObjectMapper.Map<IdentityRole, RoleDto>(role);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var db = await GetDbContextAsync();
            var role = await db.Set<IdentityRole>().FirstOrDefaultAsync(x => x.Id == id);
            if (role == null)
                return;

            var rolePermissions = db.Set<PermissionGrant>().Where(x => x.ProviderKey == role.Name);

            //删除权限缓存
            await Cache.RemoveManyAsync(rolePermissions.Select(x => PermissionGrantCacheItem.CalculateCacheKey(x.Name, PermissionValueProvider.Name, role.Name)));

            db.Set<PermissionGrant>().RemoveRange(rolePermissions);
            db.Set<IdentityRole>().Remove(role);

            await db.SaveChangesAsync();
        }
    }
}
