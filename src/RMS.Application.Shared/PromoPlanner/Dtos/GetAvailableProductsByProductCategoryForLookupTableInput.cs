using Abp.Application.Services.Dto;

namespace RMS.PromoPlanner.Dtos
{
    public sealed class GetAvailableProductsByProductCategoryForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public long? FilterId { get; set; }
        public string FilterEx { get; set; }
        public string ProductCategory { get; set; } = null;
    }
}
