using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.RegistrationFormFields.Dtos
{
    public class GetAllRegistrationFormFieldsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string DescriptionFilter { get; set; }


		 public string FormFieldDescriptionFilter { get; set; }

		 
    }
}