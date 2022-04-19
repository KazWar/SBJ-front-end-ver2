using Abp.Application.Services.Dto;

namespace RMS.SBJ.HandlingBatch.Dtos
{
    public class HandlingBatchStatusDto : EntityDto<long>
    {
        public string StatusCode { get; set; }

        public string StatusDescription { get; set; }
    }
}