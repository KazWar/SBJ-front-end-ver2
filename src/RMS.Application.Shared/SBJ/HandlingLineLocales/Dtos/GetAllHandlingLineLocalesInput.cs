using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.HandlingLineLocales.Dtos
{
    public class GetAllHandlingLineLocalesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }


		 public string HandlingLineCustomerCodeFilter { get; set; }

		 		 public string LocaleLanguageCodeFilter { get; set; }

		 
    }
}