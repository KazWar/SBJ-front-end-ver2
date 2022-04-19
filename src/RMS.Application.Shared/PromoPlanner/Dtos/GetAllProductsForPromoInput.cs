using Abp.Application.Services.Dto;

namespace RMS.PromoPlanner.Dtos
{
    public sealed class GetAllProductsForPromoInput : PagedAndSortedResultRequestDto
    {
        public long PromoId { get; set; }
    }
}
