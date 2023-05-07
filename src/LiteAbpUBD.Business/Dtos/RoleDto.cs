using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace LiteAbpUBD.Business.Dtos
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsStatic { get; set; }
        public List<string> Permissions { get; set; }
    }

    public class RolePagerQueryDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }

    public class RoleCreateOrUpdateDto
    {
        public Guid? Id { get; set; }

        [Required]
        [DynamicStringLength(typeof(IdentityRoleConsts), nameof(IdentityRoleConsts.MaxNameLength))]
        public string Name { get; set; }

        public List<string> Permissions { get; set; }
    }
}
