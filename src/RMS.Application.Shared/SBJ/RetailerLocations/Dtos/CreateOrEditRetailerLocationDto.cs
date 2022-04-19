
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.RetailerLocations.Dtos
{
    public class CreateOrEditRetailerLocationDto : EntityDto<long?>
    {

		[Required]
		public string Name { get; set; }
		
		
		 public long RetailerId { get; set; }
		 
		 		 public long? AddressId { get; set; }
		 
		 
    }
}