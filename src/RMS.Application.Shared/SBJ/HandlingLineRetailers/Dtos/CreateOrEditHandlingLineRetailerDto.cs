
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.HandlingLineRetailers.Dtos
{
    public class CreateOrEditHandlingLineRetailerDto : EntityDto<long?>
    {

		 public long HandlingLineId { get; set; }
		 
		 		 public long? RetailerId { get; set; }
		 
		 
    }
}