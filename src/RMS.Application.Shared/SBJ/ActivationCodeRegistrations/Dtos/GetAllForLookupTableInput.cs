using Abp.Application.Services.Dto;

namespace RMS.SBJ.ActivationCodeRegistrations.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}