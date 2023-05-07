using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using LiteAbpUBD.Business;
using LiteAbpUBD.Business.Services;
using LiteAbpUBD.Business.Dtos;
using Volo.Abp.Authorization.Permissions;

namespace LiteAbpUBD.Web.Controllers
{
    [Authorize(Permissions.Roles.Default)]
    public class RoleController : BaseController
    {
        protected RoleService RoleService { get; }
        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }
        public RoleController(RoleService roleService, IPermissionDefinitionManager permissionDefinitionManager)
        {
            RoleService = roleService;
            PermissionDefinitionManager = permissionDefinitionManager;
        }

        [HttpGet]
        public async Task<List<RoleDto>> GetAsync(RolePagerQueryDto dto) => await RoleService.GetListAsync(dto);
        
        [HttpPost]
        [Authorize(Permissions.Roles.Create)]
        public async Task<RoleDto> CreateAsync(RoleCreateOrUpdateDto dto) => await CreateOrUpdateAsync(dto);

        [HttpPut]
        [Authorize(Permissions.Roles.Update)]
        public async Task<RoleDto> UpdateAsync(RoleCreateOrUpdateDto dto) => await CreateOrUpdateAsync(dto);

        private async Task<RoleDto> CreateOrUpdateAsync(RoleCreateOrUpdateDto dto)=>await RoleService.CreateOrUpdateAsync(dto);
        
        [HttpDelete]
        [Authorize(Permissions.Roles.Create)]
        public async Task DeleteAsync(Guid id)=>await RoleService.DeleteAsync(id);
    }
}
