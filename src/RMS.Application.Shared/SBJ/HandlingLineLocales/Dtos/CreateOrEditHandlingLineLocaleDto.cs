
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.HandlingLineLocales.Dtos
{
    public class CreateOrEditHandlingLineLocaleDto : EntityDto<long?>
    {

		 public long HandlingLineId { get; set; }
		 
		 		 public long LocaleId { get; set; }
		 
		 
    }
}