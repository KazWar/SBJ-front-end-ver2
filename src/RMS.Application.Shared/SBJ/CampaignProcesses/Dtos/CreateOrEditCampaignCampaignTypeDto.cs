
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class CreateOrEditCampaignCampaignTypeDto : EntityDto<long?>
    {

		public string Description { get; set; }
		
		
		 public long CampaignId { get; set; }
		 
		 		 public long CampaignTypeId { get; set; }
		 
		 
    }
}