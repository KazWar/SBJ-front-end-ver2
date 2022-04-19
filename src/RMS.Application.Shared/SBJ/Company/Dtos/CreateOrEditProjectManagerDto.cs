
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Company.Dtos
{
    public class CreateOrEditProjectManagerDto : EntityDto<long?>
    {

		[Required]
		public string Name { get; set; }
		
		
		[StringLength(ProjectManagerConsts.MaxPhoneNumberLength, MinimumLength = ProjectManagerConsts.MinPhoneNumberLength)]
		public string PhoneNumber { get; set; }
		
		
		[RegularExpression(ProjectManagerConsts.EmailAddressRegex)]
		public string EmailAddress { get; set; }
		
		
		 public long AddressId { get; set; }
		 
		 
    }
}