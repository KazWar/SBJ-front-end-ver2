using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.MakitaSerialNumber.Dtos
{
    public class GetMakitaSerialNumberForEditOutput
    {
        public CreateOrEditMakitaSerialNumberDto MakitaSerialNumber { get; set; }

    }
}