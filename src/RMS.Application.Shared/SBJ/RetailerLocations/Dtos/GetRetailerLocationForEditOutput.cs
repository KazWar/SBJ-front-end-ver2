using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.RetailerLocations.Dtos
{
    public class GetRetailerLocationForEditOutput
    {
		public CreateOrEditRetailerLocationDto RetailerLocation { get; set; }

		public string RetailerName { get; set;}

		public string AddressAddressLine1 { get; set;}


    }
}