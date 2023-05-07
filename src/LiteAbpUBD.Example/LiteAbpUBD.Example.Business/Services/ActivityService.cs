
using LiteAbpUBD.Example.Business.Dtos;
using LiteAbpUBD.Example.DataAccess;
using LiteAbpUBD.Example.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.EntityFrameworkCore;

namespace LiteAbpUBD.Example.Business.Services
{
    public class ActivityService : ApplicationService
    {
        protected NoneModelBuilderDbContext NoneModelBuilderDbContext => LazyServiceProvider.LazyGetRequiredService<IDbContextProvider<NoneModelBuilderDbContext>>().GetDbContextAsync().GetAwaiter().GetResult();
        public ActivityService()
        {
        }

        public async Task<PagedResultDto<ActivityInfoDto>> GetListAsync(ActivityInfoPagerQueryDto dto)
        {
            var query = NoneModelBuilderDbContext.ActivityInfo.Where(x => !x.IsDeleted);
            query = query.WhereIf(!dto.Filter.IsNullOrWhiteSpace(), x => x.Title != null && x.Title.Contains(dto.Filter));
            dto.Sorting = string.IsNullOrWhiteSpace(dto.Sorting) ? "startTime desc" : dto.Sorting;
            query = query.OrderBy(dto.Sorting);
            var count = query.Count();
            query = query.PageBy(dto);
            var list = await query.ToListAsync();
            var dtos = ObjectMapper.Map<List<ActivityInfo>, List<ActivityInfoDto>>(list);
            return new PagedResultDto<ActivityInfoDto>(count, dtos);
        }

        public async Task<string> CreateOrUpdateAsync(ActivityInfoCreateOrUpdateDto dto)
        {
            ActivityInfo activity;
            if (dto.Id.HasValue)
            {
                activity = await NoneModelBuilderDbContext.ActivityInfo.FirstOrDefaultAsync(x => x.Id == dto.Id);
                if (activity == null)
                    throw new UserFriendlyException("活动数据不存在");
                ObjectMapper.Map(dto, activity);
                NoneModelBuilderDbContext.ActivityInfo.Update(activity);
            }
            else
            {
                activity = ObjectMapper.Map<ActivityInfoCreateOrUpdateDto, ActivityInfo>(dto);
                await NoneModelBuilderDbContext.ActivityInfo.AddAsync(activity);
            }
            await NoneModelBuilderDbContext.SaveChangesAsync();
            return activity.ConcurrencyStamp;
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var activity = await NoneModelBuilderDbContext.ActivityInfo.FirstOrDefaultAsync(x => x.Id.Equals(id));
            NoneModelBuilderDbContext.ActivityInfo.Remove(activity);
            await NoneModelBuilderDbContext.SaveChangesAsync();
        }
    }
}
