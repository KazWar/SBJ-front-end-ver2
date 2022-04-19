using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Registrations.Dtos
{
    public class GetRegistrationForEditOutput
    {
		public CreateOrEditRegistrationDto Registration { get; set; }

		public string RegistrationStatusStatusCode { get; set;}

		public string FormLocaleDescription { get; set;}


    }
}