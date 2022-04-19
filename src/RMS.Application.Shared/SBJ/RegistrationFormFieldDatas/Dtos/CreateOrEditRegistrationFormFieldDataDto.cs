
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.RegistrationFormFieldDatas.Dtos
{
    public class CreateOrEditRegistrationFormFieldDataDto : EntityDto<long?>
    {

		public string Value { get; set; }
		
		
		 public long RegistrationFormFieldId { get; set; }
		 
		 		 public long RegistrationId { get; set; }
		 
		 
    }
}