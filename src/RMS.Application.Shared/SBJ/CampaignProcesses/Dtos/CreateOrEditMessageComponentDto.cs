
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class CreateOrEditMessageComponentDto : EntityDto<long?>
    {

		public bool IsActive { get; set; }
		
		
		 public long MessageTypeId { get; set; }
		 
		 		 public long MessageComponentTypeId { get; set; }
		 
		 
    }
}