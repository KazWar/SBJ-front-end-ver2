
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Forms.Dtos
{
    public class CreateOrEditFormDto : EntityDto<long?>
    {

		public string Version { get; set; }
		
		
		 public long SystemLevelId { get; set; }
		 
		 
    }
}