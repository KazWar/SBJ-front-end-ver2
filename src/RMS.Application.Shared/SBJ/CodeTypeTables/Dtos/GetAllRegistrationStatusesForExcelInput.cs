using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class GetAllRegistrationStatusesForExcelInput
    {
		public string Filter { get; set; }

		public string StatusCodeFilter { get; set; }

		public string DescriptionFilter { get; set; }

		public int IsActiveFilter { get; set; }



    }
}