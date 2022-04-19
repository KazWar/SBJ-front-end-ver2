using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class GetCampaignTypeEventForEditOutput
    {
		public CreateOrEditCampaignTypeEventDto CampaignTypeEvent { get; set; }

		public string CampaignTypeName { get; set;}

		public string ProcessEventName { get; set;}


    }
}