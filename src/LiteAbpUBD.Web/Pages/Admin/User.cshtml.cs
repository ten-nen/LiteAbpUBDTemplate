using LiteAbpUBD.Business;
using LiteAbpUBD.Business.Dtos;
using LiteAbpUBD.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LiteAbpUBD.Web.Pages.Admin
{
    [Authorize(Permissions.Users.Default)]
    public class UserModel : PageModel
    {
        public RoleService RoleService { get; }
        public UserModel(RoleService roleService)
        {
            RoleService = roleService;
        }
        public List<RoleDto> Roles { get; set; }
        public async Task OnGetAsync()
        {
            Roles = await RoleService.GetListAsync(new RolePagerQueryDto());
        }
    }
}
