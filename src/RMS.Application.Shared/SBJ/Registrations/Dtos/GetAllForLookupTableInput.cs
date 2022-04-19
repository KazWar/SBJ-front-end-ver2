using Abp.Application.Services.Dto;

namespace RMS.SBJ.Registrations.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}