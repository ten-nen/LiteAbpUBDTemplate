using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LiteAbpUBD.DataAccess.Extensions
{
    public static class DbContextModelBuilderExtensions
    {
        public static void Configure(
        [NotNull] this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.TryConfigureObjectExtensions<DbContext>();
        }
    }
}
