using Abp.Application.Services.Dto;

namespace RMS.SBJ.Forms.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public long CurrentFormPage { get; set; }
    }
}