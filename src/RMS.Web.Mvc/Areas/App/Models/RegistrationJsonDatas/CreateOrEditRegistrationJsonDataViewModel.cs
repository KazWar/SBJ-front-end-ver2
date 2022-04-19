using RMS.SBJ.RegistrationJsonData.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.RegistrationJsonDatas
{
    public class CreateOrEditRegistrationJsonDataModalViewModel
    {
        public CreateOrEditRegistrationJsonDataDto RegistrationJsonData { get; set; }

        public string RegistrationFirstName { get; set; }

        public bool IsEditMode => RegistrationJsonData.Id.HasValue;
    }
}