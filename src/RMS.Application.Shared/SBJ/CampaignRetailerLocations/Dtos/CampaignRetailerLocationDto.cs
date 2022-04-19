
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.CampaignRetailerLocations.Dtos
{
    public class CampaignRetailerLocationDto : EntityDto<long>
    {

		 public long CampaignId { get; set; }

		 		 public long RetailerLocationId { get; set; }

		 
    }
}