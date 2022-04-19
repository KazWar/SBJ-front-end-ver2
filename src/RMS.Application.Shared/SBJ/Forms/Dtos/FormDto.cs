
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.Forms.Dtos
{
    public class FormDto : EntityDto<long>
    {
		public string Version { get; set; }


		 public long SystemLevelId { get; set; }

		 
    }
}