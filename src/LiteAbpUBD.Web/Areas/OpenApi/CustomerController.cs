using LiteAbpUBD.Example.Business;
using LiteAbpUBD.Example.Business.Dtos.OpenApi;
using LiteAbpUBD.Example.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace LiteAbpUBD.Web.Areas.OpenApi
{
    [OpenApiAuthorize(ExamplePermissions.OpenCustomers.Default)]
    public class CustomerController : OpenApiBaseController
    {
        protected CustomerService CustomerService { get; }
        public CustomerController(CustomerService customerService)
        {
            CustomerService =  customerService;
        }
        /// <summary>
        /// 添加客户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [OpenApiAuthorize(ExamplePermissions.OpenCustomers.Create)]
        public async Task CreateAsync([FromBody] OpenCustomerBatchCreateDto dto) => CustomerService.OpenCreateAsync(dto.Customers);
    }
}
