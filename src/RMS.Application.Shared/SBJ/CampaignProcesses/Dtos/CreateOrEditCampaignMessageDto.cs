
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class CreateOrEditCampaignMessageDto : EntityDto<long?>
    {

		public bool IsActive { get; set; }
		
		
		 public long CampaignId { get; set; }
		 
		 		 public long MessageId { get; set; }
		 
		 
    }
}