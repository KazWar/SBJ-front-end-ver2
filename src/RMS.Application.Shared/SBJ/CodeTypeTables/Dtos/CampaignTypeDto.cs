
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class CampaignTypeDto : EntityDto<long>
    {
		public string Name { get; set; }

        public string Code { get; set; }

        public bool IsActive { get; set; }



    }
}