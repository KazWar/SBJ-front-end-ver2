using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.Forms.Dtos
{
    public class GetAllFormFieldValueListsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string PossibleListValuesFilter { get; set; }


		 public string FormFieldDescriptionFilter { get; set; }

		 		 public string ValueListDescriptionFilter { get; set; }

		 
    }
}