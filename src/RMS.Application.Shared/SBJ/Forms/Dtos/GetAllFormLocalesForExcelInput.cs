using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.Forms.Dtos
{
    public class GetAllFormLocalesForExcelInput
    {
		public string Filter { get; set; }

		public string DescriptionFilter { get; set; }


		 public string FormVersionFilter { get; set; }

		 		 public string LocaleDescriptionFilter { get; set; }

		 
    }
}