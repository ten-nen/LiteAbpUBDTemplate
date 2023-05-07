using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

namespace LiteAbpUBD.Web.Pages.Home
{
    public class NotFoundModel : PageModel
    {
        public IActionResult OnGet()
        {
            return RedirectToPage("Error", new { HttpStatusCode = HttpStatusCode.NotFound });
        }
    }
}
