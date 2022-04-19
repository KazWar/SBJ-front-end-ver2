using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.PromoPlanner.Dtos
{
    public class GetPromoProductForEditOutput
    {
		public CreateOrEditPromoProductDto PromoProduct { get; set; }

		public string PromoPromocode { get; set;}

		public string ProductCtn { get; set;}


    }
}