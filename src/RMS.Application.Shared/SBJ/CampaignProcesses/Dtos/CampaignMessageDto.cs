
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class CampaignMessageDto : EntityDto<long>
    {
		public bool IsActive { get; set; }


		 public long CampaignId { get; set; }

		 		 public long MessageId { get; set; }

		 
    }
}