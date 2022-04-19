using Abp.AutoMapper;
using RMS.Authorization.Roles.Dto;
using RMS.Web.Areas.App.Models.Common;

namespace RMS.Web.Areas.App.Models.Roles
{
    [AutoMapFrom(typeof(GetRoleForEditOutput))]
    public class CreateOrEditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
    {
        public bool IsEditMode => Role.Id.HasValue;
    }
}