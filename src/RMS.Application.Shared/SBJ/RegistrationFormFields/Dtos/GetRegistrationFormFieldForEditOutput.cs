using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.RegistrationFormFields.Dtos
{
    public class GetRegistrationFormFieldForEditOutput
    {
		public CreateOrEditRegistrationFormFieldDto RegistrationFormField { get; set; }

		public string FormFieldDescription { get; set;}


    }
}