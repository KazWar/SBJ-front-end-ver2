using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.PromoPlanner.Dtos
{
    public class GetPromoRetailerForEditOutput
    {
		public CreateOrEditPromoRetailerDto PromoRetailer { get; set; }

		public string PromoPromocode { get; set;}

		public string RetailerCode { get; set;}


    }
}