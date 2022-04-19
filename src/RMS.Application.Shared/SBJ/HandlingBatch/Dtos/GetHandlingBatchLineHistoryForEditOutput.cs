using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.HandlingBatch.Dtos
{
    public class GetHandlingBatchLineHistoryForEditOutput
    {
        public CreateOrEditHandlingBatchLineHistoryDto HandlingBatchLineHistory { get; set; }

        public string HandlingBatchLineCustomerCode { get; set; }

        public string HandlingBatchLineStatusStatusDescription { get; set; }

    }
}