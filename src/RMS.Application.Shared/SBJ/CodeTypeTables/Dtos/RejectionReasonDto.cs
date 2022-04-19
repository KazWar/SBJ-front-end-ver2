using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class RejectionReasonDto : EntityDto<long>
    {
        public string Description { get; set; }

    }
}