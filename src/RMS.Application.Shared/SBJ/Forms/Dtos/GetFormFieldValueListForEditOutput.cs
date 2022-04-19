using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Forms.Dtos
{
    public class GetFormFieldValueListForEditOutput
    {
		public CreateOrEditFormFieldValueListDto FormFieldValueList { get; set; }

		public string FormFieldDescription { get; set;}

		public string ValueListDescription { get; set;}


    }
}