using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.HandlingBatch.Dtos
{
    public class HandlingBatchLineDto : EntityDto<long>
    {
        public int? Quantity { get; set; }

        public long HandlingBatchId { get; set; }

        public long PurchaseRegistrationId { get; set; }

        public long HandlingBatchLineStatusId { get; set; }

    }
}