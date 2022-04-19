using Abp.Application.Services.Dto;

namespace RMS.SBJ.RegistrationJsonData.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}