﻿using Abp.Application.Services.Dto;

namespace RMS.SBJ.ProductGifts.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}