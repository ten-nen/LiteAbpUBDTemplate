using LiteAbpUBD.Business;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation.Urls;

namespace LiteAbpUBD.Web
{
    [DependsOn(
        typeof(BusinessModule),
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpAutofacModule),
        typeof(AbpIdentityAspNetCoreModule),
        typeof(AbpAspNetCoreSerilogModule)
        )]
    public class WebModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            //配置站点地址
            Configure<AppUrlOptions>(options => options.Applications["Web"].RootUrl = configuration["App:SelfUrl"]);

            //配置认证
            context.Services.AddAuthentication();
            context.Services.ConfigureApplicationCookie(options =>
            {
                //options.Cookie.Name = IdentityConstants.ApplicationScheme;
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
                options.LoginPath = "/home/login";
                options.LogoutPath = "/home/logout";
                options.AccessDeniedPath = "/home/forbidden";
            });
            context.Services.AddAuthorization();

            //配置跨域
            context.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .WithOrigins(
                            configuration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .WithAbpExposedHeaders()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
            //全局取消防伪验证
            Configure<AbpAntiForgeryOptions>(options =>
            {
                //options.TokenCookie.Expiration = TimeSpan.FromDays(365);
                options.AutoValidateFilter =
                    type => false;
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAbpRequestLocalization(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("zh-Hans");
                options.SupportedCultures = new[] { new CultureInfo("zh-Hans") };
                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider()
                };
            });

            app.UseCorrelationId();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();

            app.UseUnitOfWork();
            app.UseAuthorization();

            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();

            var schemes = context.ServiceProvider.GetService<IAuthenticationSchemeProvider>();
            var sc = schemes.GetAllSchemesAsync();

        }
    }
}
