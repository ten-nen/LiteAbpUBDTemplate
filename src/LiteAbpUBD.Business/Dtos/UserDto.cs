
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace LiteAbpUBD.Business.Dtos
{
    public class UserLoginDto
    {
        [Required]
        [StringLength(255)]
        public string UserName { get; set; }

        [Required]
        [StringLength(32)]
        [DisableAuditing]
        public string Password { get; set; }

        public bool? RememberMe { get; set; }
    }
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public List<RoleDto> Roles { get; set; }
    }

    public class UserPagerQueryDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public Guid? RoleId { get; set; }
    }

    public class UserCreateOrUpdateDto
    {
        public Guid? Id { get; set; }
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
        public string UserName { get; set; }

        [DisableAuditing]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        public string Password { get; set; }

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxNameLength))]
        public string Name { get; set; }

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]
        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public List<Guid> RoleIds { get; set; }
    }
}
