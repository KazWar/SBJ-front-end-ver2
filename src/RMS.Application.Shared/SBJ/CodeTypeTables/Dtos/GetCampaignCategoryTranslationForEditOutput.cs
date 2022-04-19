using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class GetCampaignCategoryTranslationForEditOutput
    {
		public CreateOrEditCampaignCategoryTranslationDto CampaignCategoryTranslation { get; set; }

		public string LocaleDescription { get; set;}

		public string CampaignCategoryName { get; set;}


    }
}