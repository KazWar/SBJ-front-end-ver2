using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Forms.Dtos
{
    public class GetFormFieldForEditOutput
    {
		public CreateOrEditFormFieldDto FormField { get; set; }

		public string FieldTypeDescription { get; set;}


    }
}