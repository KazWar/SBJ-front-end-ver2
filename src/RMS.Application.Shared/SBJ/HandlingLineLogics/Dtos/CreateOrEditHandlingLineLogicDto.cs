
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.HandlingLineLogics.Dtos
{
    public class CreateOrEditHandlingLineLogicDto : EntityDto<long?>
    {

		public decimal FirstHandlingLineId { get; set; }
		
		
		[Required]
		public string Operator { get; set; }
		
		
		public decimal SecondHandlingLineId { get; set; }
		
		

    }
}