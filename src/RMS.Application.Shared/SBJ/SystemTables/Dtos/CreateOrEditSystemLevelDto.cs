
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.SystemTables.Dtos
{
    public class CreateOrEditSystemLevelDto : EntityDto<long?>
    {

		[Required]
		public string Description { get; set; }
		
		

    }
}