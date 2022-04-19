using RMS.SBJ.Registrations.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.Registrations
{
    public class CreateOrEditRegistrationViewModel
    {
        public CreateOrEditRegistrationDto Registration { get; set; }

        public string RegistrationStatusStatusCode { get; set; }

        public string FormLocaleDescription { get; set; }

        public List<RegistrationFormLocaleLookupTableDto> RegistrationFormLocaleList { get; set; }

        public bool IsEditMode => Registration.Id.HasValue;
    }
}