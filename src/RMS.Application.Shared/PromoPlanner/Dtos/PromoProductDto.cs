
using System;
using Abp.Application.Services.Dto;

namespace RMS.PromoPlanner.Dtos
{
    public class PromoProductDto : EntityDto<long>
    {

		 public long PromoId { get; set; }

		 		 public long ProductId { get; set; }

		 
    }
}