
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Forms.Dtos
{
    public class CreateOrEditFormLocaleDto : EntityDto<long?>
    {

		public string Description { get; set; }
		
		
		 public long FormId { get; set; }
		 
		 		 public long LocaleId { get; set; }
		 
		 
    }
}