using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.ProductGifts.Dtos
{
    public class GetProductGiftForEditOutput
    {
        public CreateOrEditProductGiftDto ProductGift { get; set; }

        public string CampaignName { get; set; }

    }
}