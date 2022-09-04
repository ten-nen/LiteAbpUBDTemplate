using LiteAbpUBD.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;

namespace LiteAbpUBD.Business
{
    public class LiteAbpUBDDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        protected LiteAbpUBDDbContext DbContext { get; }

        public LiteAbpUBDDataSeedContributor(
            LiteAbpUBDDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual Task SeedAsync(DataSeedContext context)
        {
            return Task.CompletedTask;
        }
    }
}
