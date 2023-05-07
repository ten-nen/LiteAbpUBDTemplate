using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;
using LiteAbpUBD.Example.DataAccess.Entities;

namespace LiteAbpUBD.Example.DataAccess.Extensions
{
    public static class ExampleDbContextModelBuilderExtensions
    {
        public static void ConfigureDataPlatform(
        [NotNull] this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<Customer>(b =>
            {
                b.ToTable("Customers");
                b.ConfigureByConvention();
                b.Property(x => x.Name).HasMaxLength(32).IsRequired().HasComment("客户姓名");
                b.Property(x => x.Tel).HasMaxLength(16).IsRequired().HasComment("客户电话");
                b.ApplyObjectExtensionMappings();
            });

            builder.TryConfigureObjectExtensions<ExampleDbContext>();
        }
    }
}
