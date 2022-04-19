using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class CreateOrEditRejectionReasonDto : EntityDto<long?>
    {

        [Required]
        public string Description { get; set; }

    }
}