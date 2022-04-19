
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.ProductHandlings.Dtos
{
    public class CreateOrEditProductHandlingDto : EntityDto<long?>
    {

		public string Description { get; set; }
		
		
		 public long CampaignId { get; set; }
		 
		 
    }
}