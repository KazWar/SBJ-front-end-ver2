using Abp.Application.Services.Dto;

namespace RMS.SBJ.HandlingLineRetailers.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}