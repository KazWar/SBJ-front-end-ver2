using Abp.Application.Services.Dto;

namespace RMS.PromoPlanner.Dtos
{
    public class GetAvailableForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

        public long? FilterId { get; set; }
        
        public string FilterEx { get; set; }
    }
}