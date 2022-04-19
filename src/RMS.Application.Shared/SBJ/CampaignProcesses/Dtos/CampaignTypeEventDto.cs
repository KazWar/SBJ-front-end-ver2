
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class CampaignTypeEventDto : EntityDto<long>
    {
		public int SortOrder { get; set; }


		 public long CampaignTypeId { get; set; }

		 		 public long ProcessEventId { get; set; }

		 
    }
}