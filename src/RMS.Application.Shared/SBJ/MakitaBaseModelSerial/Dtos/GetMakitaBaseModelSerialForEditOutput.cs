using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.MakitaBaseModelSerial.Dtos
{
    public class GetMakitaBaseModelSerialForEditOutput
    {
        public CreateOrEditMakitaBaseModelSerialDto MakitaBaseModelSerial { get; set; }

    }
}