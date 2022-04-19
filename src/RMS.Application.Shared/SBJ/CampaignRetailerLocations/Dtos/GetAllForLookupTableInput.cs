using Abp.Application.Services.Dto;

namespace RMS.SBJ.CampaignRetailerLocations.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}