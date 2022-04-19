
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class CreateOrEditCampaignTypeEventRegistrationStatusDto : EntityDto<long?>
    {

		public int SortOrder { get; set; }
		
		
		 public long CampaignTypeEventId { get; set; }
		 
		 		 public long RegistrationStatusId { get; set; }

		public bool IsUpdate { get; set; }
	}
}