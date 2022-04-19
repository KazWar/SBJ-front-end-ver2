using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.HandlingBatch.Dtos
{
    public class GetHandlingBatchLineStatusForEditOutput
    {
        public CreateOrEditHandlingBatchLineStatusDto HandlingBatchLineStatus { get; set; }

    }
}