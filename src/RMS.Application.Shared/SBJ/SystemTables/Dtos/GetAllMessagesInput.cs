using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.SystemTables.Dtos
{
    public class GetAllMessagesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string VersionFilter { get; set; }


		 public string SystemLevelDescriptionFilter { get; set; }

		 
    }
}