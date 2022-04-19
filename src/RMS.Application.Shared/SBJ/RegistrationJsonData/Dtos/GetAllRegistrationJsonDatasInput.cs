using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.RegistrationJsonData.Dtos
{
    public class GetAllRegistrationJsonDatasInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string DataFilter { get; set; }

        public DateTime? MaxDateCreatedFilter { get; set; }
        public DateTime? MinDateCreatedFilter { get; set; }

        public string RegistrationFirstNameFilter { get; set; }

    }
}