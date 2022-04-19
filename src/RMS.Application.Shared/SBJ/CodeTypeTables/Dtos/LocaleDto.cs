
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class LocaleDto : EntityDto<long>
    {
		public string LanguageCode { get; set; }

		public string Description { get; set; }

		public bool IsActive { get; set; }


		 public long CountryId { get; set; }

		 
    }
}