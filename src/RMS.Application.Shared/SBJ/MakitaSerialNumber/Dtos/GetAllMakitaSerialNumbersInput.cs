using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.MakitaSerialNumber.Dtos
{
    public class GetAllMakitaSerialNumbersInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string ProductCodeFilter { get; set; }

        public string SerialNumberFilter { get; set; }

        public string RetailerExternalCodeFilter { get; set; }

    }
}