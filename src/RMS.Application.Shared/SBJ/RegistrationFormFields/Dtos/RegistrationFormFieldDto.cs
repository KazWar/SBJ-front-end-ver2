
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.RegistrationFormFields.Dtos
{
    public class RegistrationFormFieldDto : EntityDto<long>
    {
		public string Description { get; set; }


		 public long FormFieldId { get; set; }

		 
    }
}