using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Products.Dtos
{
    public class GetProductCategoryForEditOutput
    {
		public CreateOrEditProductCategoryDto ProductCategory { get; set; }


    }
}