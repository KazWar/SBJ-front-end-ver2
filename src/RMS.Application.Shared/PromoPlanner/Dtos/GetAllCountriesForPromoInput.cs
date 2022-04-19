using Abp.Application.Services.Dto;

namespace RMS.PromoPlanner.Dtos
{
    public sealed class GetAllCountriesForPromoInput : PagedAndSortedResultRequestDto
    {
        public long PromoId { get; set; }
    }
}
