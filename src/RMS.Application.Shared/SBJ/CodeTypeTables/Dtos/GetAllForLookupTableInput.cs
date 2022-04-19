using Abp.Application.Services.Dto;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}