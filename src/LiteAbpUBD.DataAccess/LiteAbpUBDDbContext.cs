using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace LiteAbpUBD.DataAccess
{
    [ConnectionStringName("Default")]
    public class LiteAbpUBDDbContext :
        AbpDbContext<LiteAbpUBDDbContext>
    {
        public LiteAbpUBDDbContext(DbContextOptions<LiteAbpUBDDbContext> options)
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

            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(demoConsts.DbTablePrefix + "YourEntities", demoConsts.DbSchema);
            //    b.ConfigureByConvention(); //auto configure for the base class props
            //    //...
            //});
        }
    }

    public class EfCoreRepository<TEntity> : EfCoreRepository<LiteAbpUBDDbContext, TEntity>
    where TEntity : class, IEntity
    {
        public LiteAbpUBDDbContext CurrentDbContext { get; }
        public EfCoreRepository(IDbContextProvider<LiteAbpUBDDbContext> dbContextProvider) : base(dbContextProvider)
        {
            CurrentDbContext = dbContextProvider.GetDbContextAsync().Result;
        }
    }
}
