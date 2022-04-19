
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class CreateOrEditCampaignStepDto : EntityDto<long?>
    {

		public string Name { get; set; }
		
		
		public int SortOrder { get; set; }
		
		
		 public long? CampaignTypeId { get; set; }
		 
		 
    }
}