using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace LiteAbpUBD.Web.Pages.Home
{
    [Authorize]
    public class LogoutModel : PageModel
    {
        protected SignInManager<IdentityUser> SignInManager { get; }
        protected IdentitySecurityLogManager IdentitySecurityLogManager { get; }
        private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;
        public LogoutModel(
        SignInManager<IdentityUser> signInManager,
        IdentitySecurityLogManager identitySecurityLogManager,
        ICurrentPrincipalAccessor currentPrincipalAccessor)
        {
            SignInManager = signInManager;
            IdentitySecurityLogManager = identitySecurityLogManager;
            _currentPrincipalAccessor = currentPrincipalAccessor;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = IdentitySecurityLogActionConsts.Logout
            });

            await SignInManager.SignOutAsync();

            return RedirectToPage("Login");
        }
    }
}
