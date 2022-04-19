using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.RegistrationFormFieldDatas.Dtos
{
    public class GetAllRegistrationFormFieldDatasInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string ValueFilter { get; set; }


		 public string RegistrationFormFieldDescriptionFilter { get; set; }

		 		 public string RegistrationFirstNameFilter { get; set; }

		 
    }
}