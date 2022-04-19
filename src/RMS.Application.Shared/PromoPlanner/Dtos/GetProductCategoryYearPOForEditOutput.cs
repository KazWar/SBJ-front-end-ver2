using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.PromoPlanner.Dtos
{
    public class GetProductCategoryYearPoForEditOutput
    {
		public CreateOrEditProductCategoryYearPoDto ProductCategoryYearPo { get; set; }

		public string ProductCategoryCode { get; set;}


    }
}