
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.HandlingLineLogics.Dtos
{
    public class HandlingLineLogicDto : EntityDto<long>
    {
		public decimal FirstHandlingLineId { get; set; }

		public string Operator { get; set; }

		public decimal SecondHandlingLineId { get; set; }



    }
}