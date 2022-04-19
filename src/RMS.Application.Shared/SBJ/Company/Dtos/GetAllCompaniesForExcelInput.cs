using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.Company.Dtos
{
    public class GetAllCompaniesForExcelInput
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public string PhoneNumberFilter { get; set; }

		public string EmailAddressFilter { get; set; }

		public string BICCashBackFilter { get; set; }

		public string IBANCashBackFilter { get; set; }


		 public string AddressPostalCodeFilter { get; set; }

		 
    }
}