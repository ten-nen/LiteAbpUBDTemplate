using LiteAbpUBD.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LiteAbpUBD.Web.Pages.Admin
{
    [Authorize(Permissions.Indexs.Default)]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
