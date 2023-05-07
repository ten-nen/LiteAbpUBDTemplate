using LiteAbpUBD.Example.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LiteAbpUBD.Example.Business
{
    public class ExampleDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        protected ExampleDbContext DbContext { get; }

        public ExampleDataSeedContributor(
            ExampleDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual Task SeedAsync(DataSeedContext context)
        {
            return Task.CompletedTask;
        }
    }
}
