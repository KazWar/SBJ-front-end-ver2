
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Retailers.Dtos
{
    public class CreateOrEditRetailerDto : EntityDto<long?>
    {

		public string Name { get; set; }
		
		
		public string Code { get; set; }
		
		
		 public long? CountryId { get; set; }
		 
		 
    }
}