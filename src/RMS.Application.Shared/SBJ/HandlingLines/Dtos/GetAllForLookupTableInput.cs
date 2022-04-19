using Abp.Application.Services.Dto;

namespace RMS.SBJ.HandlingLines.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}