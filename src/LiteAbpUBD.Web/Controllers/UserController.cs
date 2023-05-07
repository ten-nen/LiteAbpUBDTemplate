
using LiteAbpUBD.Business;
using LiteAbpUBD.Business.Dtos;
using LiteAbpUBD.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace LiteAbpUBD.Web.Controllers
{
    [Authorize(Permissions.Users.Default)]
    public class UserController : BaseController
    {
        protected UserService UserService { get; }
        protected IdentityUserManager UserManager { get; }
        public UserController(
             IdentityUserManager userManager,
            UserService userService)
        {
            UserService = userService;
            UserManager = userManager;
        }

        [HttpGet]
        public async Task<PagedResultDto<UserDto>> GetAsync(UserPagerQueryDto dto) => await UserService.GetListAsync(dto);

        [HttpPost]
        [Authorize(Permissions.Users.Create)]
        public async Task<UserDto> CreateAsync(UserCreateOrUpdateDto dto) => await CreateOrUpdateAsync(dto);

        [HttpPut]
        [Authorize(Permissions.Users.Update)]
        public async Task<UserDto> UpdateAsync(UserCreateOrUpdateDto dto) => await CreateOrUpdateAsync(dto);

        private async Task<UserDto> CreateOrUpdateAsync(UserCreateOrUpdateDto dto) => await UserService.CreateOrUpdateAsync(dto);

        [HttpDelete]
        [Authorize(Permissions.Users.Delete)]
        public async Task DeleteAsync(Guid id) => await UserService.DeleteAsync(id);
    }
}
