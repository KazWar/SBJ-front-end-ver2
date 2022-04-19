
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class CampaignCampaignTypeDto : EntityDto<long>
    {
		public string Description { get; set; }


		 public long CampaignId { get; set; }

		 		 public long CampaignTypeId { get; set; }

		 
    }
}