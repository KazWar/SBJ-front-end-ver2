
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.CodeTypeTables.Dtos
{
    public class MessageTypeDto : EntityDto<long>
    {
		public string Name { get; set; }

		public string Source { get; set; }

		public bool IsActive { get; set; }


		 public long MessageId { get; set; }

		 
    }
}