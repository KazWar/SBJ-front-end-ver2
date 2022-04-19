
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class CreateOrEditCampaignTypeEventDto : EntityDto<long?>
    {

		public int SortOrder { get; set; }
		
		
		 public long CampaignTypeId { get; set; }
		 
		 		 public long ProcessEventId { get; set; }
		public bool IsUpdate { get; set; }

	}
}