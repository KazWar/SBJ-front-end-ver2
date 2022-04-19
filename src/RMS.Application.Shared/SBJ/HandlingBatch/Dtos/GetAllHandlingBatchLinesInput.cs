using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.HandlingBatch.Dtos
{
    public class GetAllHandlingBatchLinesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxQuantityFilter { get; set; }
        public int? MinQuantityFilter { get; set; }

        public string HandlingBatchRemarksFilter { get; set; }

        public string PurchaseRegistrationInvoiceImagePathFilter { get; set; }

        public string HandlingBatchLineStatusStatusDescriptionFilter { get; set; }

    }
}