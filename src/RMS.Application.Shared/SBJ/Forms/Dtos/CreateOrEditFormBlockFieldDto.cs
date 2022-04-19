
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Forms.Dtos
{
    public class CreateOrEditFormBlockFieldDto : EntityDto<long?>
    {

		public int SortOrder { get; set; }
		
		
		 public long? FormFieldId { get; set; }
		 
		 		 public long? FormBlockId { get; set; }
		 
		 
    }
}