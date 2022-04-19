
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.RegistrationFormFieldDatas.Dtos
{
    public class RegistrationFormFieldDataDto : EntityDto<long>
    {
		public string Value { get; set; }


		 public long RegistrationFormFieldId { get; set; }

		 		 public long RegistrationId { get; set; }

		 
    }
}