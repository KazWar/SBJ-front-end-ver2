using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.Forms.Dtos
{
    public class GetAllFormsForExcelInput
    {
		public string Filter { get; set; }

		public string VersionFilter { get; set; }


		 public string SystemLevelDescriptionFilter { get; set; }

		 
    }
}