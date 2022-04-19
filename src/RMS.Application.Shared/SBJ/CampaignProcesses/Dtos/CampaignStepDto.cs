
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class CampaignStepDto : EntityDto<long>
    {
		public string Name { get; set; }

		public int SortOrder { get; set; }


		 public long? CampaignTypeId { get; set; }

		 
    }
}