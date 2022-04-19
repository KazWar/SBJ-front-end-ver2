using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.ProductHandlings.Dtos
{
    public class GetProductHandlingForEditOutput
    {
		public CreateOrEditProductHandlingDto ProductHandling { get; set; }

		public string CampaignName { get; set;}


    }
}