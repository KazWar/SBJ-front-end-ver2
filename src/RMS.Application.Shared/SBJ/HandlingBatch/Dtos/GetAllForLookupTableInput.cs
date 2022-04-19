using Abp.Application.Services.Dto;

namespace RMS.SBJ.HandlingBatch.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}