using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.Forms.Dtos
{
    public class GetAllListValueTranslationsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string KeyValueFilter { get; set; }

		public string DescriptionFilter { get; set; }


		 public string ListValueKeyValueFilter { get; set; }

		 		 public string LocaleLanguageCodeFilter { get; set; }

		 
    }
}