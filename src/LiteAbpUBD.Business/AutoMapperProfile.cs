using AutoMapper;
using LiteAbpUBD.Business.Dtos;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;

namespace LiteAbpUBD.Business
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<IdentityRole, RoleDto>().ForMember(x=>x.Permissions,opt=>opt.Ignore());

            CreateMap<PermissionGrant, PermissionDto>();

            CreateMap<IdentityUser, UserDto>().ForMember(x => x.Roles, opt => opt.Ignore());
            CreateMap<UserCreateOrUpdateDto, IdentityUser>().ForMember(x => x.Roles, opt => opt.Ignore());
        }
    }
}
