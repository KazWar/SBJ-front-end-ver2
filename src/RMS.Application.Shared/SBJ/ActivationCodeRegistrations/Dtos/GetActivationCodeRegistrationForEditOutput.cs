using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.ActivationCodeRegistrations.Dtos
{
    public class GetActivationCodeRegistrationForEditOutput
    {
		public CreateOrEditActivationCodeRegistrationDto ActivationCodeRegistration { get; set; }

		public string ActivationCodeCode { get; set;}

		public string RegistrationFirstName { get; set;}


    }
}