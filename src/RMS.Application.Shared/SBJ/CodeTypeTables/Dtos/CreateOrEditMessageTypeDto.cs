
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class CreateOrEditMessageTypeDto : EntityDto<long?>
    {

		[Required]
		public string Name { get; set; }
		
		
		[Required]
		public string Source { get; set; }
		
		
		public bool IsActive { get; set; }
		
		
		 public long MessageId { get; set; }
		 
		 
    }
}