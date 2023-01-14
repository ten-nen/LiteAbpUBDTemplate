using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.Swashbuckle;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using LiteAbpUBD.Business;
using Microsoft.OpenApi.Models;

namespace LiteAbpUBD.Web
{
    [DependsOn(
        typeof(BusinessModule),
        typeof(AbpSwashbuckleModule),
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

            Configure<KestrelServerOptions>(options => options.AllowSynchronousIO = true);
            Configure<IISServerOptions>(options => options.AllowSynchronousIO = true);

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

            //配置Swagger
            context.Services.AddAbpSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "LiteAbpUBD API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                    {
                        Description = @"Authorization header using the ApiKey scheme. \r\n\r\n 
                      Enter 'ApiSign' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'ApiKey 12345abcdef'",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "ApiKey",
                    });

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement(){
                                                    {
                                                     new OpenApiSecurityScheme{Reference = new OpenApiReference{Type = ReferenceType.SecurityScheme,Id = "ApiKey"}},new string[]{ }
                                                    }});

                    //options.IncludeXmlComments(Path.Combine(hostingEnvironment.ContentRootPath, "api.web.xml"), true);
                    //options.IncludeXmlComments(Path.Combine(hostingEnvironment.ContentRootPath, "api.business.xml"));
                }
            );

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

            app.UseUnitOfWork();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseAbpSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "LiteAbpUBD API");
            });

            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();

        }
    }
}
