using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.HandlingBatch.Dtos
{
    public class GetHandlingBatchLineForEditOutput
    {
        public CreateOrEditHandlingBatchLineDto HandlingBatchLine { get; set; }

        public string HandlingBatchRemarks { get; set; }

        public string PurchaseRegistrationInvoiceImagePath { get; set; }

        public string HandlingBatchLineStatusStatusDescription { get; set; }

    }
}