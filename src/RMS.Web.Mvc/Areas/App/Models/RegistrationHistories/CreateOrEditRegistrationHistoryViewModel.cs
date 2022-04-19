using RMS.SBJ.RegistrationHistory.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.RegistrationHistories
{
    public class CreateOrEditRegistrationHistoryModalViewModel
    {
        public CreateOrEditRegistrationHistoryDto RegistrationHistory { get; set; }

        public string RegistrationStatusStatusCode { get; set; }

        public string RegistrationFirstName { get; set; }

        public bool IsEditMode => RegistrationHistory.Id.HasValue;
    }
}