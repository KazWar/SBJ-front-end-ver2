using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.HandlingBatch.Dtos
{
    public class CreateOrEditHandlingBatchDto : EntityDto<long?>
    {

        public DateTime DateCreated { get; set; }

        public string Remarks { get; set; }

        public long CampaignTypeId { get; set; }

        public long HandlingBatchStatusId { get; set; }

    }
}