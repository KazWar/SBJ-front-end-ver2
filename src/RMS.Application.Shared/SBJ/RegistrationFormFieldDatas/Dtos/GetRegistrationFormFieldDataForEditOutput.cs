using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.RegistrationFormFieldDatas.Dtos
{
    public class GetRegistrationFormFieldDataForEditOutput
    {
		public CreateOrEditRegistrationFormFieldDataDto RegistrationFormFieldData { get; set; }

		public string RegistrationFormFieldDescription { get; set;}

		public string RegistrationFirstName { get; set;}


    }
}