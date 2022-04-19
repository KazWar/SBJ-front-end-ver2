using Abp.Application.Services.Dto;

namespace RMS.SBJ.ActivationCodes.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}