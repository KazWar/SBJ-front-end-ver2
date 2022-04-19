
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.Forms.Dtos
{
    public class FormLocaleDto : EntityDto<long>
    {
		public string Description { get; set; }


		 public long FormId { get; set; }

		 		 public long LocaleId { get; set; }

		 
    }
}