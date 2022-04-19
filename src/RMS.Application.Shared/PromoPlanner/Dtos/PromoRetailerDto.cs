
using System;
using Abp.Application.Services.Dto;

namespace RMS.PromoPlanner.Dtos
{
    public class PromoRetailerDto : EntityDto<long>
    {

		 public long PromoId { get; set; }

		 		 public long RetailerId { get; set; }

		 
    }
}