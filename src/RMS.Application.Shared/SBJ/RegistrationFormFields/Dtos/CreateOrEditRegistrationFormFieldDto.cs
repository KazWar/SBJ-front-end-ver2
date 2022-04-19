
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.RegistrationFormFields.Dtos
{
    public class CreateOrEditRegistrationFormFieldDto : EntityDto<long?>
    {

		public string Description { get; set; }
		
		
		 public long FormFieldId { get; set; }
		 
		 
    }
}