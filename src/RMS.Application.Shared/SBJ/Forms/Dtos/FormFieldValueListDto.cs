
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.Forms.Dtos
{
    public class FormFieldValueListDto : EntityDto<long>
    {
		public string PossibleListValues { get; set; }


		 public long FormFieldId { get; set; }

		 		 public long ValueListId { get; set; }

		 
    }
}