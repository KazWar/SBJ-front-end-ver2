
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class CreateOrEditCampaignCategoryTranslationDto : EntityDto<long?>
    {

		[Required]
		public string Name { get; set; }
		
		
		 public long LocaleId { get; set; }
		 
		 		 public long CampaignCategoryId { get; set; }
		 
		 
    }
}