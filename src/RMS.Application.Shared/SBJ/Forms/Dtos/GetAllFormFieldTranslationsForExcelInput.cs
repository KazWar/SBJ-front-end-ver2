using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.Forms.Dtos
{
    public class GetAllFormFieldTranslationsForExcelInput
    {
		public string Filter { get; set; }

		public string LabelFilter { get; set; }

		public string DefaultValueFilter { get; set; }

		public string RegularExpressionFilter { get; set; }


		 public string FormFieldDescriptionFilter { get; set; }

		 		 public string LocaleLanguageCodeFilter { get; set; }

		 
    }
}