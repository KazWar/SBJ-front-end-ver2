using Abp.Application.Services.Dto;

namespace RMS.SBJ.RegistrationFormFields.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}