using LiteAbpUBD.DataAccess;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace LiteAbpUBD.Business
{
    public class DataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        protected DbContext DbContext { get; }

        public DataSeedContributor(
            DbContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual Task SeedAsync(DataSeedContext context)
        {
            return Task.CompletedTask;
        }
    }
}
