﻿using Abp.Application.Services.Dto;

namespace RMS.SBJ.Retailers.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}