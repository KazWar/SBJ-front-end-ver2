using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.RegistrationHistory.Dtos
{
    public class GetRegistrationHistoryForEditOutput
    {
        public CreateOrEditRegistrationHistoryDto RegistrationHistory { get; set; }

        public string RegistrationStatusStatusCode { get; set; }

        public string RegistrationFirstName { get; set; }

    }
}