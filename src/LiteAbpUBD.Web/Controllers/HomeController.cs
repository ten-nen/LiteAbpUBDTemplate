using LiteAbpUBD.Business;
using LiteAbpUBD.Business.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace LiteAbpUBD.Web.Controllers
{
    [Route("~/", Name = "default")]
    [Route("Home")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : AbpController
    {
        protected SignInManager<IdentityUser> SignInManager { get; }
        protected IdentityUserManager UserManager { get; }
        protected IdentitySecurityLogManager IdentitySecurityLogManager { get; }

        public HomeController(
        SignInManager<IdentityUser> signInManager,
        IdentityUserManager userManager,
        IdentitySecurityLogManager identitySecurityLogManager)
        {
            SignInManager = signInManager;
            UserManager = userManager;
            IdentitySecurityLogManager = identitySecurityLogManager;
        }

        [Route("NotFound")]
        public new IActionResult NotFound() => View("Error", HttpStatusCode.NotFound);

        [Route("Forbidden")]
        public IActionResult Forbidden() => View("Error", HttpStatusCode.Forbidden);

        [Route("Error")]
        public IActionResult Error(HttpStatusCode code = HttpStatusCode.InternalServerError) => View(code);

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Login")]
        public async Task<IActionResult> LoginAsync(UserLoginDto dto)
        {
            if (Request.IsAjax())
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
                    throw new UserFriendlyException("登录失败" + (signInResult.IsLockedOut || signInResult.IsNotAllowed || signInResult.RequiresTwoFactor ? null : "，账号或密码错误"));
                }

                return new OkResult();
            }
            return View();
        }

        [HttpGet]
        [Route("Logout")]
        [Authorize]
        public virtual async Task<IActionResult> Logout()
        {
            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = IdentitySecurityLogActionConsts.Logout
            });

            await SignInManager.SignOutAsync();

            return RedirectToAction("Login", "Home");
        }
    }
}
