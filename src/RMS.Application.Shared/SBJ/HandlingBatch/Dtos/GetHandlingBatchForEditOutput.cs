using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.HandlingBatch.Dtos
{
    public class GetHandlingBatchForEditOutput
    {
        public CreateOrEditHandlingBatchDto HandlingBatch { get; set; }

        public string CampaignTypeName { get; set; }

        public string HandlingBatchStatusStatusDescription { get; set; }

    }
}