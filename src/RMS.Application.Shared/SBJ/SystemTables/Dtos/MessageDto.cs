
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.SystemTables.Dtos
{
    public class MessageDto : EntityDto<long>
    {
		public string Version { get; set; }


		 public long SystemLevelId { get; set; }

		 
    }
}