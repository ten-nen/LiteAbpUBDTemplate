using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace LiteAbpUBD.Web.Areas.OpenApi
{
    [Area("openapi")]
    [Route("api/openapi/[controller]")]
    public abstract class OpenApiBaseController : AbpControllerBase
    {
    }
}
