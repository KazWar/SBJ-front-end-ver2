using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Company.Dtos
{
    public class GetAddressForEditOutput
    {
		public CreateOrEditAddressDto Address { get; set; }

		public string CountryCountryCode { get; set;}


    }
}