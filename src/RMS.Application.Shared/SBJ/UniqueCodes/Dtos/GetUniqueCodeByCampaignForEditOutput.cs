using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.UniqueCodes.Dtos
{
    public class GetUniqueCodeByCampaignForEditOutput
    {
        public CreateOrEditUniqueCodeByCampaignDto UniqueCodeByCampaign { get; set; }

        public string CampaignName { get; set; }

    }
}