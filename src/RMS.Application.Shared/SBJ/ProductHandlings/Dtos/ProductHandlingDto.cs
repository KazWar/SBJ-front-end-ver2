
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.ProductHandlings.Dtos
{
    public class ProductHandlingDto : EntityDto<long>
    {
		public string Description { get; set; }


		 public long CampaignId { get; set; }

		 
    }
}