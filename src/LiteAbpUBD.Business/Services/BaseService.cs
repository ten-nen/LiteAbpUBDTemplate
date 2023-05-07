
using LiteAbpUBD.DataAccess;
using Volo.Abp.Application.Services;
using Volo.Abp.EntityFrameworkCore;

namespace LiteAbpUBD.Business.Services
{
    public abstract class BaseService : ApplicationService
    {
        [Obsolete("use GetDbContextAsync")]
        protected DbContext DbContext => LazyServiceProvider.LazyGetRequiredService<IDbContextProvider<DbContext>>().GetDbContextAsync().GetAwaiter().GetResult();
        protected async Task<DbContext> GetDbContextAsync()=>await LazyServiceProvider.LazyGetRequiredService<IDbContextProvider<DbContext>>().GetDbContextAsync();
    }
}
