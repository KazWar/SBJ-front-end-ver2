using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.HandlingLineProducts.Dtos
{
    public class GetHandlingLineProductForEditOutput
    {
		public CreateOrEditHandlingLineProductDto HandlingLineProduct { get; set; }

		public string HandlingLineCustomerCode { get; set;}

		public string ProductDescription { get; set;}


    }
}