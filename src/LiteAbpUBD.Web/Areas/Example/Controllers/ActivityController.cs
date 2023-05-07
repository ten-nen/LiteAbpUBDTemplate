using LiteAbpUBD.Example.Business;
using LiteAbpUBD.Example.Business.Dtos;
using LiteAbpUBD.Example.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace LiteAbpUBD.Web.Areas.Example.Controllers
{
    [Authorize]
    public class ActivityController : ExampleBaseController
    {
        protected ActivityService ActivityService { get; }
        public ActivityController(
            ActivityService activityService)
        {
            ActivityService = activityService;
        }

        [HttpGet]
        [Authorize(ExamplePermissions.Activitys.Default)]
        public async Task<PagedResultDto<ActivityInfoDto>> GetAsync(ActivityInfoPagerQueryDto dto)=> await ActivityService.GetListAsync(dto);
        
        [HttpPost]
        [Authorize(ExamplePermissions.Activitys.Create)]
        public async Task<string> CreateAsync(ActivityInfoCreateOrUpdateDto dto) => await SaveOrUpdateAsync(dto);

        [HttpPut]
        [Authorize(ExamplePermissions.Activitys.Update)]
        public async Task<string> UpdateAsync(ActivityInfoCreateOrUpdateDto dto) => await SaveOrUpdateAsync(dto);
        private async Task<string> SaveOrUpdateAsync(ActivityInfoCreateOrUpdateDto dto) => await ActivityService.CreateOrUpdateAsync(dto);

        [HttpDelete]
        [Authorize(ExamplePermissions.Activitys.Delete)]
        public async Task DeleteAsync(Guid id)=>await ActivityService.DeleteByIdAsync(id);
    }
}
