using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation.Urls;
using Microsoft.OpenApi.Models;
using Volo.Abp.Swashbuckle;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Volo.Abp.Data;
using Volo.Abp.Identity;
using LiteAbpUBD.Common;
using LiteAbpUBD.Business;
using Volo.Abp.AspNetCore.Mvc.Localization;
using LiteAbpUBD.Business.Localization;
using Volo.Abp.VirtualFileSystem;
using LiteAbpUBD.DataAccess;
using LiteAbpUBD.Example.Business;
using LiteAbpUBD.Example.DataAccess;
using Volo.Abp.Auditing;

namespace LiteAbpUBD.Web
{
    [DependsOn(
        typeof(CommonModule),
        typeof(BusinessModule),
        typeof(ExampleBusinessModule),
        typeof(AbpSwashbuckleModule),
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpAutofacModule),
        typeof(AbpIdentityAspNetCoreModule),
        typeof(AbpAspNetCoreSerilogModule)
        )]
    public class WebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(BusinessResource), typeof(BusinessModule).Assembly);
                options.AddAssemblyResource(typeof(ExampleBusinessResource), typeof(ExampleBusinessModule).Assembly);
            });
        }
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            //配置站点地址
            Configure<AppUrlOptions>(options => options.Applications["Web"].RootUrl = configuration["App:SelfUrl"]);

            Configure<KestrelServerOptions>(options => options.AllowSynchronousIO = true);
            Configure<IISServerOptions>(options => options.AllowSynchronousIO = true);

            //实体历史记录
            Configure<AbpAuditingOptions>(options =>
            {
                options.EntityHistorySelectors.Add(
                    new NamedTypeSelector(
                        "HistoryRecordedEntity",
                        type => typeof(IHistoryRecordedEntity).IsAssignableFrom(type)
                    )
                );
            });

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
                                                     new OpenApiSecurityScheme{Reference = new OpenApiReference{Type = ReferenceType.SecurityScheme,Id = "ApiKey"}},Array.Empty<string>()
                                                    }});
                    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{typeof(WebModule).Assembly.GetName().Name}.xml"), true);
                    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{typeof(BusinessModule).Assembly.GetName().Name}.xml"));
                    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{typeof(ExampleBusinessModule).Assembly.GetName().Name}.xml"));
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
            ////全局取消防伪验证
            //Configure<AbpAntiForgeryOptions>(options =>
            //{
            //    //options.TokenCookie.Expiration = TimeSpan.FromDays(365);
            //    options.AutoValidateFilter =
            //        type => false;
            //});
            //Configure<RazorPagesOptions>(options => options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute()));

            if (hostingEnvironment.IsDevelopment())
            {
                //开发中处理嵌入的文件
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<BusinessModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}LiteAbpUBD.Business"));
                    options.FileSets.ReplaceEmbeddedByPhysical<ExampleBusinessModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}LiteAbpUBD.Example{Path.DirectorySeparatorChar}LiteAbpUBD.Example.Business"));
                });
            }
        }

        public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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

            await SeedDataAsync(context);
        }

        private static async Task SeedDataAsync(ApplicationInitializationContext context)
        {
            using var scope = context.ServiceProvider.CreateScope();
            await scope.ServiceProvider
                .GetRequiredService<IDataSeeder>()
                .SeedAsync(new DataSeedContext(null)
                     .WithProperty(IdentityDataSeedContributor.AdminEmailPropertyName, IdentityDataSeedContributor.AdminEmailDefaultValue)
                     .WithProperty(IdentityDataSeedContributor.AdminPasswordPropertyName, "123456"));
        }

    }
}
