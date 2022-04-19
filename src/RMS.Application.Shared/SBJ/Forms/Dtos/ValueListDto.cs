
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.Forms.Dtos
{
    public class ValueListDto : EntityDto<long>
    {
		public string Description { get; set; }

		public string ListValueApiCall { get; set; }



    }
}