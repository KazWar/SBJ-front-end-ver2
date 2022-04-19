using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.ProductGifts.Dtos
{
    public class CreateOrEditProductGiftDto : EntityDto<long?>
    {

        public long CampaignId { get; set; }

    }
}