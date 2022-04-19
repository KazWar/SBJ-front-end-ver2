using Abp.Application.Services.Dto;

namespace RMS.SBJ.ProductHandlings.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}