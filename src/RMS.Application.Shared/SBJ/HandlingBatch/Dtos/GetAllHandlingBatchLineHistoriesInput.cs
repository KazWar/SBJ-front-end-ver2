using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.HandlingBatch.Dtos
{
    public class GetAllHandlingBatchLineHistoriesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string HandlingBatchLineCustomerCodeFilter { get; set; }

        public string HandlingBatchLineStatusStatusDescriptionFilter { get; set; }

    }
}