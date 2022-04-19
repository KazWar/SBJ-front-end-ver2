
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class CreateOrEditMessageVariableDto : EntityDto<long?>
    {

		[Required]
		public string Description { get; set; }
		
		
		[Required]
		public string RmsTable { get; set; }
		
		
		[Required]
		public string TableField { get; set; }
		
		

    }
}