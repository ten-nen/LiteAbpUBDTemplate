using AutoMapper;
using LiteAbpUBD.Business.Dtos;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Data;
using LiteAbpUBD.Common.Consts;

namespace LiteAbpUBD.Business
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<IdentityRole, RoleDto>().ForMember(x => x.Permissions, opt => opt.Ignore());

            CreateMap<IdentityUser, UserDto>()
                .ForMember(x => x.ApiSecret, opt => opt.MapFrom(y => y.GetProperty(IdentityUserExtentPropertyConsts.ApiSecret, "")))
                .ForMember(x => x.Roles, opt => opt.Ignore());
            CreateMap<UserCreateOrUpdateDto, IdentityUser>().ForMember(x => x.Roles, opt => opt.Ignore());

        }
    }
}
