using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.RegistrationJsonData.Dtos
{
    public class CreateOrEditRegistrationJsonDataDto : EntityDto<long?>
    {

        public string Data { get; set; }

        public DateTime DateCreated { get; set; }

        public long? RegistrationId { get; set; }

    }
}