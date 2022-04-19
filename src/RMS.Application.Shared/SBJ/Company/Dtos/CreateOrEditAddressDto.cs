
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Company.Dtos
{
    public class CreateOrEditAddressDto : EntityDto<long?>
    {

		[Required]
		public string AddressLine1 { get; set; }
		
		
		public string AddressLine2 { get; set; }
		
		
		[Required]
		[StringLength(AddressConsts.MaxPostalCodeLength, MinimumLength = AddressConsts.MinPostalCodeLength)]
		public string PostalCode { get; set; }
		
		
		[Required]
		public string City { get; set; }
		
		
		 public long CountryId { get; set; }
		 
		 
    }
}