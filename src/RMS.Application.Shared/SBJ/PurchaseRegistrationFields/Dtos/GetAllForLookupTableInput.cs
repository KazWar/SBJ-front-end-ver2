using Abp.Application.Services.Dto;

namespace RMS.SBJ.PurchaseRegistrationFields.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}