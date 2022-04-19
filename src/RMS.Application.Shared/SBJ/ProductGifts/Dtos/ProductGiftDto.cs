using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.ProductGifts.Dtos
{
    public class ProductGiftDto : EntityDto<long>
    {

        public long CampaignId { get; set; }

    }
}