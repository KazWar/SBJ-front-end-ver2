using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.RegistrationJsonData.Dtos
{
    public class RegistrationJsonDataDto : EntityDto<long>
    {
        public string Data { get; set; }

        public DateTime DateCreated { get; set; }

        public long? RegistrationId { get; set; }

    }
}