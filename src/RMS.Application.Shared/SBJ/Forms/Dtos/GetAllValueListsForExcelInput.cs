﻿using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.Forms.Dtos
{
    public class GetAllValueListsForExcelInput
    {
		public string Filter { get; set; }

		public string DescriptionFilter { get; set; }

		public string ListValueApiCallFilter { get; set; }



    }
}