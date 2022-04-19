
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Forms.Dtos
{
    public class CreateOrEditFormFieldValueListDto : EntityDto<long?>
    {

		public string PossibleListValues { get; set; }
		
		
		 public long FormFieldId { get; set; }
		 
		 		 public long ValueListId { get; set; }
		 
		 
    }
}