using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.HandlingLineRetailers.Dtos
{
    public class GetHandlingLineRetailerForEditOutput
    {
		public CreateOrEditHandlingLineRetailerDto HandlingLineRetailer { get; set; }

		public string HandlingLineCustomerCode { get; set;}

		public string RetailerName { get; set;}


    }
}