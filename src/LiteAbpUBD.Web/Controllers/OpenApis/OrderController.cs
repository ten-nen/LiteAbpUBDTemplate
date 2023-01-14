using LiteAbpUBD.Business;
using LiteAbpUBD.Business.Dtos.OpenApis;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace LiteAbpUBD.Web.Controllers.OpenApis
{
    /// <summary>
    /// 订单
    /// </summary>
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "OpenApis")]
    [OpenApiAuthorize(PermissionConsts.开放接口)]
    public class OrderController : AbpControllerBase
    {
        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [OpenApiAuthorize(PermissionConsts.开放接口_新增订单)]
        public void Create([FromBody] OpenOrderCreateDto dto)
        {

        }
    }
}
