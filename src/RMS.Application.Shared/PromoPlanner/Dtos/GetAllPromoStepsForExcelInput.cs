using Abp.Application.Services.Dto;
using System;

namespace RMS.PromoPlanner.Dtos
{
    public class GetAllPromoStepsForExcelInput
    {
		public string Filter { get; set; }

		public short? MaxSequenceFilter { get; set; }
		public short? MinSequenceFilter { get; set; }

		public string DescriptionFilter { get; set; }



    }
}