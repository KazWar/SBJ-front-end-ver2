
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.ActivationCodeRegistrations.Dtos
{
    public class ActivationCodeRegistrationDto : EntityDto<long>
    {

		 public long ActivationCodeId { get; set; }

		 		 public long RegistrationId { get; set; }

		 
    }
}