using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class GetAllMessageVariablesForExcelInput
    {
		public string Filter { get; set; }

		public string DescriptionFilter { get; set; }

		public string RmsTableFilter { get; set; }

		public string TableFieldFilter { get; set; }



    }
}