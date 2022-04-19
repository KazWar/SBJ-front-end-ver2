using Abp.Application.Services.Dto;

namespace RMS.SBJ.HandlingLineLocales.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}