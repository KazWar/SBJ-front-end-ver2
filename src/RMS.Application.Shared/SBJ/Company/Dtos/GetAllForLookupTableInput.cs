using Abp.Application.Services.Dto;

namespace RMS.SBJ.Company.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}