using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class GetMessageContentTranslationForEditOutput
    {
		public CreateOrEditMessageContentTranslationDto MessageContentTranslation { get; set; }

		public string LocaleDescription { get; set;}

		public string MessageComponentContentContent { get; set;}


    }
}