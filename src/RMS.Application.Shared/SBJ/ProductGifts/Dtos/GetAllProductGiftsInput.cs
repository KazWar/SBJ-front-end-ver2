using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.ProductGifts.Dtos
{
    public class GetAllProductGiftsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string CampaignNameFilter { get; set; }

    }
}