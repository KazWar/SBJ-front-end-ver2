using Abp.Application.Services.Dto;

namespace RMS.SBJ.PurchaseRegistrationFieldDatas.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}