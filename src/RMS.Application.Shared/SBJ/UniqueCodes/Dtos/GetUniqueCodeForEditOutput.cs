using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.UniqueCodes.Dtos
{
    public class GetUniqueCodeForEditOutput
    {
        public CreateOrEditUniqueCodeDto UniqueCode { get; set; }

    }
}