using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.ActivationCodeRegistrations.Dtos
{
    public class GetAllActivationCodeRegistrationsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }


		 public string ActivationCodeCodeFilter { get; set; }

		 		 public string RegistrationFirstNameFilter { get; set; }

		 
    }
}