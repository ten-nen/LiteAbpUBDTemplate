
using LiteAbpUBD.Business.Dtos;
using LiteAbpUBD.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;

namespace LiteAbpUBD.Business.Services
{
    public class UserService : ApplicationService
    {
        protected LiteAbpUBDDbContext DbContext => LazyServiceProvider.LazyGetRequiredService<IDbContextProvider<LiteAbpUBDDbContext>>().GetDbContextAsync().Result;
        protected IdentityUserManager UserManager { get; }
        public UserService(
            IdentityUserManager userManager)
        {
            UserManager = userManager;
        }

        public virtual PagedResultDto<UserDto> GetList(UserPagerQueryDto dto)
        {
            var query = DbContext.Set<IdentityUser>().Include(x => x.Roles).Where(x => !x.IsDeleted);
            query = query.WhereIf(!dto.Filter.IsNullOrWhiteSpace(), x => (x.UserName != null && x.UserName.Contains(dto.Filter)) || (x.Name != null && x.Name.Contains(dto.Filter)));
            query = query.WhereIf(dto.RoleId.HasValue, x => x.Roles.Any(r => r.RoleId == dto.RoleId.Value));
            var count = query.Count();
            query = query.PageBy(dto.SkipCount, dto.MaxResultCount);
            var users = query.ToList();
            var roleIds = users.SelectMany(x => x.Roles.Select(r => r.RoleId));
            var roles = DbContext.Set<IdentityRole>().Where(x => roleIds.Contains(x.Id)).ToList();
            var userDtos = users.Select(x =>
            {
                var userDto = ObjectMapper.Map<IdentityUser, UserDto>(x);
                userDto.Roles = ObjectMapper.Map<IEnumerable<IdentityRole>, List<RoleDto>>(roles.Where(r => x.Roles.Any(u => u.RoleId == r.Id)));
                return userDto;
            }).ToList();
            return new PagedResultDto<UserDto>(count, userDtos);
        }

        public virtual UserDto CreateOrUpdate(UserCreateOrUpdateDto dto)
        {
            IdentityUser user;
            if (dto.Id.HasValue)
            {
                user = UserManager.FindByIdAsync(dto.Id.Value.ToString()).Result;
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
                    result = UserManager.RemovePasswordAsync(user).Result;
                    if (!result.Succeeded)
                        throw new AbpIdentityResultException(result);
                    UserManager.PasswordValidators.Clear();
                    result = UserManager.AddPasswordAsync(user, dto.Password).Result;
                    if (!result.Succeeded)
                        throw new AbpIdentityResultException(result);
                }
                result = UserManager.UpdateAsync(user).Result;
                if (!result.Succeeded)
                    throw new AbpIdentityResultException(result);
            }
            else
            {
                var exists = UserManager.FindByNameAsync(dto.UserName).Result != null;
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

                var result = UserManager.CreateAsync(user, dto.Password, false).Result;
                if (!result.Succeeded)
                    throw new AbpIdentityResultException(result);
            }

            CurrentUnitOfWork.SaveChangesAsync().Wait();

            return ObjectMapper.Map<IdentityUser, UserDto>(user);
        }

        public virtual void Delete(Guid id)
        {
            var user = DbContext.Set<IdentityUser>().FirstOrDefault(x => x.Id == id);
            if (user == null)
                return;
            user.IsDeleted = true;
            DbContext.Set<IdentityUser>().Update(user);
            DbContext.SaveChanges();
        }
    }
}