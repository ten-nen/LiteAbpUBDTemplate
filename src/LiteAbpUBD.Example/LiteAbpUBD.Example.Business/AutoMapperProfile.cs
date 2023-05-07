using AutoMapper;
using LiteAbpUBD.Example.Business.Dtos;
using LiteAbpUBD.Example.Business.Dtos.OpenApi;
using LiteAbpUBD.Example.DataAccess.Entities;

namespace LiteAbpUBD.Example.Business
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region OpenApiDto
            CreateMap<OpenCustomerCreateDto, Customer>().ForCtorParam("id", opt => opt.MapFrom(x => Guid.NewGuid()));
            #endregion

            #region 活动
            CreateMap<ActivityInfo, ActivityInfoDto>();
            CreateMap<ActivityInfoCreateOrUpdateDto, ActivityInfo>();
            #endregion
        }
    }
}
