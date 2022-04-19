using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.UniqueCodes.Dtos
{
    public class GetAllUniqueCodesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? UsedFilter { get; set; }

    }
}