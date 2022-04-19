using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.HandlingBatch.Dtos
{
    public class GetAllHandlingBatchStatusesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

    }
}