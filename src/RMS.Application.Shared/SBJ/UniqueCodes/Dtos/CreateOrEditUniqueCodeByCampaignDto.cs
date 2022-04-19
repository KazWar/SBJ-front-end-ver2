using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.UniqueCodes.Dtos
{
    public class CreateOrEditUniqueCodeByCampaignDto : EntityDto<string>
    {

        public bool Used { get; set; }

        public long CampaignId { get; set; }

    }
}