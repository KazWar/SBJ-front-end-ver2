
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.HandlingLineLocales.Dtos
{
    public class HandlingLineLocaleDto : EntityDto<long>
    {

		 public long HandlingLineId { get; set; }

		 		 public long LocaleId { get; set; }

		 
    }
}