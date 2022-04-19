using Abp.Application.Services.Dto;

namespace RMS.SBJ.HandlingLineProducts.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}