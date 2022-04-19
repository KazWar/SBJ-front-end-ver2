using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.UniqueCodes.Dtos
{
    public class UniqueCodeDto : EntityDto<string>
    {
        public bool Used { get; set; }

    }
}