using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.MakitaBaseModelSerial.Dtos
{
    public class GetAllMakitaBaseModelSerialsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

    }
}