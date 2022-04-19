
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class CreateOrEditProcessEventDto : EntityDto<long?>
    {

		[Required]
		public string Name { get; set; }
		
		
		public bool IsActive { get; set; }
		
		

    }
}