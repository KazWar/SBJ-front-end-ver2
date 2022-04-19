using Abp.Application.Services.Dto;

namespace RMS.SBJ.Products.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}