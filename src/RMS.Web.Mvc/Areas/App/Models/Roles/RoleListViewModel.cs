using System.Collections.Generic;
using Abp.Application.Services.Dto;
using RMS.Authorization.Permissions.Dto;
using RMS.Web.Areas.App.Models.Common;

namespace RMS.Web.Areas.App.Models.Roles
{
    public class RoleListViewModel : IPermissionsEditViewModel
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}