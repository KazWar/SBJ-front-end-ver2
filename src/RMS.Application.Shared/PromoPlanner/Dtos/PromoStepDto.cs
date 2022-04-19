
using System;
using Abp.Application.Services.Dto;

namespace RMS.PromoPlanner.Dtos
{
    public class PromoStepDto : EntityDto
    {
		public short Sequence { get; set; }

		public string Description { get; set; }



    }
}