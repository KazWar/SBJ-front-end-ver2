using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.RegistrationHistory.Dtos
{
    public class CreateOrEditRegistrationHistoryDto : EntityDto<long?>
    {

        public DateTime DateCreated { get; set; }

        public string Remarks { get; set; }

        public long AbpUserId { get; set; }

        public long RegistrationStatusId { get; set; }

        public long RegistrationId { get; set; }

    }
}