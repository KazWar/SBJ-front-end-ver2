using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Retailers.Dtos
{
    public class GetRetailerForEditOutput
    {
		public CreateOrEditRetailerDto Retailer { get; set; }

		public string CountryCountryCode { get; set;}


    }
}