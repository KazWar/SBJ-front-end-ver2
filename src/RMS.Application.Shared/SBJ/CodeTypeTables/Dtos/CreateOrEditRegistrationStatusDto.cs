
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class CreateOrEditRegistrationStatusDto : EntityDto<long?>
    {

		[Required]
		public string StatusCode { get; set; }
		
		
		[Required]
		public string Description { get; set; }
		
		
		public bool IsActive { get; set; }
		
		

    }
}