
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.ActivationCodeRegistrations.Dtos
{
    public class CreateOrEditActivationCodeRegistrationDto : EntityDto<long?>
    {

		 public long ActivationCodeId { get; set; }
		 
		 		 public long RegistrationId { get; set; }
		 
		 
    }
}