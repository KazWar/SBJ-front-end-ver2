using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Company.Dtos
{
    public class GetProjectManagerForEditOutput
    {
		public CreateOrEditProjectManagerDto ProjectManager { get; set; }

		public string AddressPostalCode { get; set;}


    }
}