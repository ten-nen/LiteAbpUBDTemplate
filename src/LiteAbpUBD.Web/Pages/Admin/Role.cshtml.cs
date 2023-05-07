using LiteAbpUBD.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LiteAbpUBD.Web.Pages.Admin
{
    [Authorize(Permissions.Roles.Default)]
    public class RoleModel : PageModel
    {     
        public void OnGet()
        {
        }
    }
}
