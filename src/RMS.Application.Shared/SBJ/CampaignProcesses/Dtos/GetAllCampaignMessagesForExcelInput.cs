﻿using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class GetAllCampaignMessagesForExcelInput
    {
		public string Filter { get; set; }

		public int? IsActiveFilter { get; set; }


		 public string CampaignNameFilter { get; set; }

		 		 public string MessageVersionFilter { get; set; }

		 
    }
}