
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.HandlingLineRetailers.Dtos
{
    public class HandlingLineRetailerDto : EntityDto<long>
    {

		 public long HandlingLineId { get; set; }

		 		 public long? RetailerId { get; set; }

		 
    }
}