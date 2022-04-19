using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class GetAllMessageContentTranslationsForExcelInput
    {
		public string Filter { get; set; }

		public string ContentFilter { get; set; }


		 public string LocaleDescriptionFilter { get; set; }

		 		 public string MessageComponentContentContentFilter { get; set; }

		 
    }
}