
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class CountryDto : EntityDto<long>
    {
		public string CountryCode { get; set; }

		public string Description { get; set; }



    }
}