
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class ProcessEventDto : EntityDto<long>
    {
		public string Name { get; set; }

		public bool IsActive { get; set; }



    }
}