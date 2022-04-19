using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.RegistrationHistory.Dtos
{
    public class GetAllRegistrationHistoriesForExcelInput
    {
        public string Filter { get; set; }

        public DateTime? MaxDateCreatedFilter { get; set; }
        public DateTime? MinDateCreatedFilter { get; set; }

        public string RemarksFilter { get; set; }

        public long? MaxAbpUserIdFilter { get; set; }
        public long? MinAbpUserIdFilter { get; set; }

        public string RegistrationStatusStatusCodeFilter { get; set; }

        public string RegistrationFirstNameFilter { get; set; }

    }
}