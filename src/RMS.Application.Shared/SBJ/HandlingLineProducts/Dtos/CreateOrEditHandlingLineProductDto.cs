
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.HandlingLineProducts.Dtos
{
    public class CreateOrEditHandlingLineProductDto : EntityDto<long?>
    {

		 public long HandlingLineId { get; set; }
		 
		 public long ProductId { get; set; }
    }
}