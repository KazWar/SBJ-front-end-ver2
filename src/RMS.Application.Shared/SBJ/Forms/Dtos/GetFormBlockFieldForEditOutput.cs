using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Forms.Dtos
{
    public class GetFormBlockFieldForEditOutput
    {
		public CreateOrEditFormBlockFieldDto FormBlockField { get; set; }

		public string FormFieldDescription { get; set;}

		public string FormBlockDescription { get; set;}


    }
}