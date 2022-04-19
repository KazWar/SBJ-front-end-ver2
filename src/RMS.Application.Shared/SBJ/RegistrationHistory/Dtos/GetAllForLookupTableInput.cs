using Abp.Application.Services.Dto;

namespace RMS.SBJ.RegistrationHistory.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}