using LiteAbpUBD.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LiteAbpUBD.Web.Pages.Admin
{
    [Authorize(Permissions.Menus.Default)]
    public class MenuModel : PageModel
    {
        public string Type { get {
                return string.IsNullOrWhiteSpace(Request.Query["type"]) ? "0" : Request.Query["type"];
            } }
        public void OnGet()
        {

        }
    }
}
