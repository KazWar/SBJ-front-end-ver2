using Abp.Application.Services.Dto;

namespace RMS.SBJ.MakitaBaseModelSerial.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}