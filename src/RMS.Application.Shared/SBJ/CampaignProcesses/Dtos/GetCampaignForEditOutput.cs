using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class GetCampaignForEditOutput
    {
		public CreateOrEditCampaignDto Campaign { get; set; }
    }
}