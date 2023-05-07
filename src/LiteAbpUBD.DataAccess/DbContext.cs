using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using LiteAbpUBD.DataAccess.Extensions;

namespace LiteAbpUBD.DataAccess
{
    [ConnectionStringName("Default")]
    public class DbContext :
        AbpDbContext<DbContext>
    {
        public DbContext(DbContextOptions<DbContext> options)
           : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Include modules to your migration db context */

            builder.ConfigurePermissionManagement();
            builder.ConfigureSettingManagement();
            builder.ConfigureAuditLogging();
            builder.ConfigureIdentity();

            /* Configure your own tables/entities inside here */

            builder.Configure();
        }
    }
}
