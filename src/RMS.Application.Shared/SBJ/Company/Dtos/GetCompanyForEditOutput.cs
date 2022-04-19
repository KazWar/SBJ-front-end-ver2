using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Company.Dtos
{
    public class GetCompanyForEditOutput
    {
		public CreateOrEditCompanyDto Company { get; set; }

		public string AddressPostalCode { get; set;}


    }
}