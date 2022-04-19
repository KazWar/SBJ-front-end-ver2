
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class CampaignTypeEventRegistrationStatusDto : EntityDto<long>
    {
		public int SortOrder { get; set; }


		 public long CampaignTypeEventId { get; set; }

		 		 public long RegistrationStatusId { get; set; }

		 
    }
}