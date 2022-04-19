
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Forms.Dtos
{
    public class CreateOrEditListValueDto : EntityDto<long?>
    {

		public string KeyValue { get; set; }
		
		
		public string Description { get; set; }
		
		
		public int SortOrder { get; set; }
		
		
		 public long ValueListId { get; set; }
		 
		 
    }
}