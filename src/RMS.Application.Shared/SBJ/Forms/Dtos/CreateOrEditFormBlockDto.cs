
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Forms.Dtos
{
    public class CreateOrEditFormBlockDto : EntityDto<long?>
    {

		public string Description { get; set; }
		
		
		public bool IsPurchaseRegistration { get; set; }
		
		
		public int SortOrder { get; set; }
		
		
		 public long FormLocaleId { get; set; }
		 
		 
    }
}