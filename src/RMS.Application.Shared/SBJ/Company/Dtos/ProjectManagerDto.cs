
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.Company.Dtos
{
    public class ProjectManagerDto : EntityDto<long>
    {
		public string Name { get; set; }

		public string PhoneNumber { get; set; }

		public string EmailAddress { get; set; }


		 public long AddressId { get; set; }

		 
    }
}