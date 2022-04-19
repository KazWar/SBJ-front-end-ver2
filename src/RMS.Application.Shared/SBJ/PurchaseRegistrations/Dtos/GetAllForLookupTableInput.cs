using Abp.Application.Services.Dto;

namespace RMS.SBJ.PurchaseRegistrations.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}