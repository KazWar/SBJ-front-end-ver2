
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class CampaignCategoryTranslationDto : EntityDto<long>
    {
		public string Name { get; set; }


		 public long LocaleId { get; set; }

		 		 public long CampaignCategoryId { get; set; }

		 
    }
}