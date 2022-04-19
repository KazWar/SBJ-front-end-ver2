using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.MakitaSerialNumber.Dtos
{
    public class CreateOrEditMakitaSerialNumberDto : EntityDto<long?>
    {

        public string ProductCode { get; set; }

        public string SerialNumber { get; set; }

        public string RetailerExternalCode { get; set; }

    }
}