using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using System.Text;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Data;
using Microsoft.AspNetCore.Identity;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Identity;
using NuGet.Protocol;
using NuGet.ProjectModel;
using System.Net;
using LiteAbpUBD.Business;

namespace LiteAbpUBD.Web.Controllers.OpenApis
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class OpenApiAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        protected readonly string PermissionName;
        public OpenApiAuthorizeAttribute(string permissionName)
        {
            PermissionName = permissionName;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var env = context.GetRequiredService<IHostEnvironment>();
            var result = new ContentResult() { StatusCode = (int)HttpStatusCode.Forbidden, ContentType = "text/html" };
            if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var auth) || auth.IsNullOrEmpty())
            {
                if (env.IsDevelopment())
                    result.Content = "Authorization为空";
                context.Result = result;
                return;
            }
            var apiKeyStr = auth.ToString().ToLower().Replace("apikey ", "");
            if (string.IsNullOrWhiteSpace(apiKeyStr) || !Guid.TryParse(apiKeyStr, out var apiKey))
            {
                if (env.IsDevelopment())
                    result.Content = "缺少apikey";
                context.Result = result;
                return;
            }

            string signStr = null;
            string sign = null;
            string tsStr = null;
            if (context.HttpContext.Request.Method.ToLower().Equals("get", StringComparison.OrdinalIgnoreCase))
            {
                sign = context.HttpContext.Request.Query["sign"].ToString();
                tsStr = context.HttpContext.Request.Query["timestamp"].ToString();

                signStr = context.HttpContext.Request.Query.Where(x => x.Key.ToLower() != "sign" && x.Key.ToLower() != "timestamp").Select(x => x.Key).OrderBy(x => x).JoinAsString("");

            }
            else
            {
                if (!context.HttpContext.Request.HasJsonContentType())
                {
                    if (env.IsDevelopment())
                        result.Content = "从body中读取json失败";
                    context.Result = result;
                    return;
                }
                string body = null;
                context.HttpContext.Request.EnableBuffering();

                var reader = new StreamReader(context.HttpContext.Request.Body, Encoding.UTF8);
                body = reader.ReadToEnd();
                context.HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);

                if (string.IsNullOrWhiteSpace(body))
                {
                    if (env.IsDevelopment())
                        result.Content = "从body中读取json失败";
                    context.Result = result;
                    return;
                }
                var jtoken = JToken.Parse(body);
                if (jtoken is not JObject joject)
                {
                    if (env.IsDevelopment())
                        result.Content = "从body中读取json失败";
                    context.Result = result;
                    return;
                }

                sign = joject.GetValue<string>("sign");
                tsStr = joject.GetValue<string>("timestamp");
                joject.Remove("sign");
                joject.Remove("timestamp");
                signStr = joject.ToJson();
            }


            if (string.IsNullOrWhiteSpace(sign))
            {
                if (env.IsDevelopment())
                    result.Content = "签名错误";
                context.Result = result;
                return;
            }
            var nowTs = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            if (string.IsNullOrWhiteSpace(tsStr) || !long.TryParse(tsStr, out var ts) || nowTs - ts > 60 || ts - nowTs > 60)
            {
                if (env.IsDevelopment())
                    result.Content = "请求过期";
                context.Result = result;
                return;
            }

            //验证用户
            var userManager = context.GetRequiredService<IdentityUserManager>();
            var user = userManager.GetByIdAsync(apiKey).Result;
            if (user == null || !user.IsActive || user.IsDeleted || (user.LockoutEnabled && user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.Now))
            {
                if (env.IsDevelopment())
                    result.Content = "认证失败";
                context.Result = result;
                return;
            }

            //验证签名
            var apiSecret = user.GetProperty<string>("ApiSecret");
            var _sign = BusinessStatics.MD5Hash($"{apiSecret}{signStr}timestamp{tsStr}{apiSecret}".ToLower());
            if (!_sign.Equals(sign, StringComparison.OrdinalIgnoreCase))
            {
                if (env.IsDevelopment())
                    result.Content = "签名错误";
                context.Result = result;
                return;
            }


            var signInManager = context.GetRequiredService<SignInManager<IdentityUser>>();
            var userPrincipal = signInManager.CreateUserPrincipalAsync(user).Result;
            context.HttpContext.User = userPrincipal;
            //验证权限
            var authorizationService = context.GetRequiredService<IAuthorizationService>();
            if (!authorizationService.AuthorizeAsync(PermissionName).Result.Succeeded)
            {
                if (env.IsDevelopment())
                    result.Content = "缺少授权";
                context.Result = result;
                return;
            }

        }
    }
}
