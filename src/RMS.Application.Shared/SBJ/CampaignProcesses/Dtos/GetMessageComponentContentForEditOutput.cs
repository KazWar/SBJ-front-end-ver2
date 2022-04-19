using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class GetMessageComponentContentForEditOutput
    {
		public CreateOrEditMessageComponentContentDto MessageComponentContent { get; set; }

		public string MessageComponentIsActive { get; set;}

		public string CampaignTypeEventRegistrationStatusSortOrder { get; set;}


    }
}