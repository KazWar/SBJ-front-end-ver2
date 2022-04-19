using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CampaignRetailerLocations.Dtos
{
    public class GetCampaignRetailerLocationForEditOutput
    {
		public CreateOrEditCampaignRetailerLocationDto CampaignRetailerLocation { get; set; }

		public string CampaignName { get; set;}

		public string RetailerLocationName { get; set;}


    }
}