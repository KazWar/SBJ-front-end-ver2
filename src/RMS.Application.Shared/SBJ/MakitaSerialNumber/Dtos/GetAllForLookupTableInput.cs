using Abp.Application.Services.Dto;

namespace RMS.SBJ.MakitaSerialNumber.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}