
using LiteAbpUBD.Business.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.Identity;

namespace LiteAbpUBD.Business.Services
{
    public class UserService : BaseService
    {
        protected IdentityUserManager UserManager { get; }
        public UserService(
            IdentityUserManager userManager)
        {
            UserManager = userManager;
        }

        public virtual async Task<PagedResultDto<UserDto>> GetListAsync(UserPagerQueryDto dto)
        {
            var db = await GetDbContextAsync();
            var query = db.Set<IdentityUser>().Include(x => x.Roles).Where(x => !x.IsDeleted);
            query = query.WhereIf(!dto.Filter.IsNullOrWhiteSpace(), x => (x.UserName != null && x.UserName.Contains(dto.Filter)) || (x.Name != null && x.Name.Contains(dto.Filter)));
            query = query.WhereIf(dto.RoleId.HasValue, x => x.Roles.Any(r => r.RoleId == dto.RoleId.Value));
            var count = await query.CountAsync();
            query = query.PageBy(dto.SkipCount, dto.MaxResultCount);
            var users = await query.ToListAsync();
            var roleIds = users.SelectMany(x => x.Roles.Select(r => r.RoleId));
            var roles = await db.Set<IdentityRole>().Where(x => roleIds.Contains(x.Id)).ToListAsync();
            var userDtos = users.Select(x =>
            {
                var userDto = ObjectMapper.Map<IdentityUser, UserDto>(x);
                userDto.ApiSecret = x.GetProperty<string>(nameof(userDto.ApiSecret));
                userDto.Roles = ObjectMapper.Map<IEnumerable<IdentityRole>, List<RoleDto>>(roles.Where(r => x.Roles.Any(u => u.RoleId == r.Id)));
                return userDto;
            }).ToList();
            return new PagedResultDto<UserDto>(count, userDtos);
        }

        public virtual async Task<UserDto> CreateOrUpdateAsync(UserCreateOrUpdateDto dto)
        {
            IdentityUser user;
            if (dto.Id.HasValue)
            {
                user = await UserManager.FindByIdAsync(dto.Id.Value.ToString());
                if (user == null)
                    throw new UserFriendlyException("用户不存在");

                if (!string.Equals(dto.Name, user.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    user.Name = dto.Name;
                }
                if (!string.Equals(user.PhoneNumber, dto.PhoneNumber, StringComparison.InvariantCultureIgnoreCase))
                {
                    user.SetPhoneNumber(dto.PhoneNumber, false);
                }
                if (dto.IsActive != user.IsActive)
                {
                    user.SetIsActive(dto.IsActive);
                }

                if (!string.IsNullOrEmpty(dto.ApiSecret))
                    user.SetProperty(nameof(dto.ApiSecret), dto.ApiSecret, false);

                user.Roles.Clear();
                if (dto.RoleIds != null)
                {
                    dto.RoleIds.ForEach(x => user.AddRole(x));
                }

                IdentityResult result;
                if (!string.IsNullOrWhiteSpace(dto.Password))
                {
                    result = await UserManager.RemovePasswordAsync(user);
                    if (!result.Succeeded)
                        throw new AbpIdentityResultException(result);
                    UserManager.PasswordValidators.Clear();
                    result = await UserManager.AddPasswordAsync(user, dto.Password);
                    if (!result.Succeeded)
                        throw new AbpIdentityResultException(result);
                }
                result = await UserManager.UpdateAsync(user);
                if (!result.Succeeded)
                    throw new AbpIdentityResultException(result);
            }
            else
            {
                var exists = await UserManager.FindByNameAsync(dto.UserName) != null;
                if (exists)
                    throw new UserFriendlyException("用户名已存在");

                user = new IdentityUser(
                   GuidGenerator.Create(),
                   dto.UserName,
                   dto.UserName + "@abp.io",
                   CurrentTenant.Id
               );
                if (dto.RoleIds != null)
                {
                    dto.RoleIds.ForEach(x => user.AddRole(x));
                }

                ObjectMapper.Map(dto, user);

                if (!string.IsNullOrEmpty(dto.ApiSecret))
                    user.SetProperty(nameof(dto.ApiSecret), dto.ApiSecret, false);

                var result = await UserManager.CreateAsync(user, dto.Password, false);
                if (!result.Succeeded)
                    throw new AbpIdentityResultException(result);
            }

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityUser, UserDto>(user);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var db = await GetDbContextAsync();
            var user = await db.Set<IdentityUser>().FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                return;
            user.IsDeleted = true;
            db.Set<IdentityUser>().Update(user);
            await db.SaveChangesAsync();
        }

    }
}