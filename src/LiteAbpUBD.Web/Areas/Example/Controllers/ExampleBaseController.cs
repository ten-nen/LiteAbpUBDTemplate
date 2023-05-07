using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace LiteAbpUBD.Web.Areas.Example.Controllers
{
    [Area("example")]
    [Route("api/example/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public abstract class ExampleBaseController : AbpControllerBase
    {
       
    }
}
