using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

namespace LiteAbpUBD.Web.Pages.Home
{
    public class ErrorModel : PageModel
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public IActionResult OnGet(HttpStatusCode httpStatusCode)
        {
            HttpStatusCode = httpStatusCode;
            return Page();
        }
    }
}
