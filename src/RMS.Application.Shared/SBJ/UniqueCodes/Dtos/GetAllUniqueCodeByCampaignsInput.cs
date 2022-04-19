using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.UniqueCodes.Dtos
{
    public class GetAllUniqueCodeByCampaignsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? UsedFilter { get; set; }

        public string CampaignNameFilter { get; set; }

    }
}