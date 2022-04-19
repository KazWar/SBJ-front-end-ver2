
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CampaignRetailerLocations.Dtos
{
    public class CreateOrEditCampaignRetailerLocationDto : EntityDto<long?>
    {

		 public long CampaignId { get; set; }
		 
		 		 public long RetailerLocationId { get; set; }
		 
		 
    }
}