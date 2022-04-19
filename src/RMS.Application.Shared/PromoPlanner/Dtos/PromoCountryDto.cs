
using System;
using Abp.Application.Services.Dto;

namespace RMS.PromoPlanner.Dtos
{
    public class PromoCountryDto : EntityDto<long>
    {

		 public long PromoId { get; set; }

		 		 public long CountryId { get; set; }

		 
    }
}