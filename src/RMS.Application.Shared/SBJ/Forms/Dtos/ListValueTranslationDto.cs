
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.Forms.Dtos
{
    public class ListValueTranslationDto : EntityDto<long>
    {
		public string KeyValue { get; set; }

		public string Description { get; set; }


		 public long ListValueId { get; set; }

		 		 public long LocaleId { get; set; }

		 
    }
}