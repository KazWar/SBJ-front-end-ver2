using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class GetAllRejectionReasonsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string DescriptionFilter { get; set; }

    }
}