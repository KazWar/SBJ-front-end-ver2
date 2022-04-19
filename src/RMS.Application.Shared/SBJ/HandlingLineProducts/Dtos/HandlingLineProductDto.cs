
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.HandlingLineProducts.Dtos
{
    public class HandlingLineProductDto : EntityDto<long>
    {

		 public long HandlingLineId { get; set; }

		 public long ProductId { get; set; }
    }
}