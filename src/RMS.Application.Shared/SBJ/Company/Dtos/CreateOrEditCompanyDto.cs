
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Company.Dtos
{
    public class CreateOrEditCompanyDto : EntityDto<long?>
    {

		[Required]
		[StringLength(CompanyConsts.MaxNameLength, MinimumLength = CompanyConsts.MinNameLength)]
		public string Name { get; set; }
		
		
		public string PhoneNumber { get; set; }
		
		
		[RegularExpression(CompanyConsts.EmailAddressRegex)]
		public string EmailAddress { get; set; }
		
		
		[RegularExpression(CompanyConsts.BicCashBackRegex)]
		public string BICCashBack { get; set; }
		
		
		[RegularExpression(CompanyConsts.IbanCashBackRegex)]
		public string IBANCashBack { get; set; }
		
		
		 public long AddressId { get; set; }
		 
		 
    }
}