
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.Company.Dtos
{
    public class AddressDto : EntityDto<long>
    {
		public string AddressLine1 { get; set; }

		public string AddressLine2 { get; set; }

		public string PostalCode { get; set; }

		public string City { get; set; }


		 public long CountryId { get; set; }

		 
    }
}