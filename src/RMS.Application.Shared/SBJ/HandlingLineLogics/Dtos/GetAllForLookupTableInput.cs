using Abp.Application.Services.Dto;

namespace RMS.SBJ.HandlingLineLogics.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}