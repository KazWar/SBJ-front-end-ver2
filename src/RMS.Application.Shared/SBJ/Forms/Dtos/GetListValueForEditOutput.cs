using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Forms.Dtos
{
    public class GetListValueForEditOutput
    {
		public CreateOrEditListValueDto ListValue { get; set; }

		public string ValueListDescription { get; set;}


    }
}