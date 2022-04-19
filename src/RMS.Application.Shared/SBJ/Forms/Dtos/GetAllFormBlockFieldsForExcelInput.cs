using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.Forms.Dtos
{
    public class GetAllFormBlockFieldsForExcelInput
    {
		public string Filter { get; set; }

		public int? MaxSortOrderFilter { get; set; }
		public int? MinSortOrderFilter { get; set; }


		 public string FormFieldDescriptionFilter { get; set; }

		 		 public string FormBlockDescriptionFilter { get; set; }

		 
    }
}