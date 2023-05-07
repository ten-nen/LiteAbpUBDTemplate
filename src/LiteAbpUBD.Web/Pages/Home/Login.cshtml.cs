using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using LiteAbpUBD.Business.Dtos;
using Volo.Abp.Identity.AspNetCore;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LiteAbpUBD.Web.Pages.Home
{
    public class LoginModel : PageModel
    {
        protected SignInManager<IdentityUser> SignInManager { get; }
        protected IdentityUserManager UserManager { get; }
        protected IdentitySecurityLogManager IdentitySecurityLogManager { get; }
        public LoginModel(
        SignInManager<IdentityUser> signInManager,
        IdentityUserManager userManager,
        IdentitySecurityLogManager identitySecurityLogManager)
        {
            SignInManager = signInManager;
            UserManager = userManager;
            IdentitySecurityLogManager = identitySecurityLogManager;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync(UserLoginDto dto)
        {
            var signInResult = await SignInManager.PasswordSignInAsync(dto.UserName, dto.Password, dto.RememberMe.HasValue && dto.RememberMe.Value, true);

            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = signInResult.ToIdentitySecurityLogAction(),
                UserName = dto.UserName
            });
            if (!signInResult.Succeeded)
            {
                throw new UserFriendlyException("µ«¬º ß∞‹" + (signInResult.IsLockedOut || signInResult.IsNotAllowed || signInResult.RequiresTwoFactor ? null : "£¨’À∫≈ªÚ√‹¬Î¥ÌŒÛ"));
            }
            return new OkResult();
        }
    }
}
