using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.MakitaSerialNumber.Dtos
{
    public class MakitaSerialNumberDto : EntityDto<long>
    {
        public string ProductCode { get; set; }

        public string SerialNumber { get; set; }

        public string RetailerExternalCode { get; set; }

    }
}