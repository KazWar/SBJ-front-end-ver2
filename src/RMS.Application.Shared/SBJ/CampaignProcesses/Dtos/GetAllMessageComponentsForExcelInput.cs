using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class GetAllMessageComponentsForExcelInput
    {
		public string Filter { get; set; }

		public int IsActiveFilter { get; set; }


		 public string MessageTypeNameFilter { get; set; }

		 		 public string MessageComponentTypeNameFilter { get; set; }

		 
    }
}