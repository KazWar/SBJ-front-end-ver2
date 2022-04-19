using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.RegistrationJsonData.Dtos
{
    public class GetRegistrationJsonDataForEditOutput
    {
        public CreateOrEditRegistrationJsonDataDto RegistrationJsonData { get; set; }

        public string RegistrationFirstName { get; set; }

    }
}