using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class GetRejectionReasonForEditOutput
    {
        public CreateOrEditRejectionReasonDto RejectionReason { get; set; }

    }
}