using Abp.Application.Services.Dto;

namespace RMS.SBJ.UniqueCodes.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}