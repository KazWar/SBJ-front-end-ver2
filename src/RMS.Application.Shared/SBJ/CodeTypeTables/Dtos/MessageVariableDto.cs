
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class MessageVariableDto : EntityDto<long>
    {
		public string Description { get; set; }

		public string RmsTable { get; set; }

		public string TableField { get; set; }



    }
}