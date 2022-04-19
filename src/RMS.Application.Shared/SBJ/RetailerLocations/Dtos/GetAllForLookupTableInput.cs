using Abp.Application.Services.Dto;

namespace RMS.SBJ.RetailerLocations.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}