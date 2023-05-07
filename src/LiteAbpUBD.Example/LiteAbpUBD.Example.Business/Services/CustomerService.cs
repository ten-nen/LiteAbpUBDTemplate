
using Volo.Abp.Application.Services;
using Volo.Abp.EntityFrameworkCore;
using LiteAbpUBD.Example.DataAccess;
using LiteAbpUBD.Example.DataAccess.Entities;
using LiteAbpUBD.Example.Business.Dtos.OpenApi;

namespace LiteAbpUBD.Example.Business.Services
{
    public class CustomerService : ApplicationService
    {
        protected ExampleDbContext DbContext => LazyServiceProvider.LazyGetRequiredService<IDbContextProvider<ExampleDbContext>>().GetDbContextAsync().GetAwaiter().GetResult();

        public virtual async Task OpenCreateAsync(IEnumerable<OpenCustomerCreateDto> dtos)
        {
            var customers = ObjectMapper.Map<IEnumerable<OpenCustomerCreateDto>, IEnumerable<Customer>>(dtos);
            await DbContext.Set<Customer>().AddRangeAsync(customers);
            await UnitOfWorkManager.Current.SaveChangesAsync();
        }
    }
}
