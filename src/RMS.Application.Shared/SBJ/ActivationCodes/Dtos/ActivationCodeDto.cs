
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.ActivationCodes.Dtos
{
    public class ActivationCodeDto : EntityDto<long>
    {
		public string Code { get; set; }

		public string Description { get; set; }


		 public long LocaleId { get; set; }

		 
    }
}