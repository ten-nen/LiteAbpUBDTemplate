using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using LiteAbpUBD.Example.DataAccess.Entities;

namespace LiteAbpUBD.Example.DataAccess
{
    [ConnectionStringName("NoneModelBuilder")]
    public class NoneModelBuilderDbContext :
        AbpDbContext<NoneModelBuilderDbContext>
    {

        public virtual DbSet<ActivityInfo> ActivityInfo { get; set; }
        public NoneModelBuilderDbContext(DbContextOptions<NoneModelBuilderDbContext> options)
           : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
