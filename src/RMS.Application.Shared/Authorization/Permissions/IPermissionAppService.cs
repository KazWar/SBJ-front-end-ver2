using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.Authorization.Permissions.Dto;

namespace RMS.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
