
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.PromoPlanner.Dtos
{
    public class CreateOrEditPromoRetailerDto : EntityDto<long?>
    {

		 public long PromoId { get; set; }
		 
		 		 public long RetailerId { get; set; }
		 
		 
    }
}