
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.SystemTables.Dtos
{
    public class SystemLevelDto : EntityDto<long>
    {
		public string Description { get; set; }



    }
}