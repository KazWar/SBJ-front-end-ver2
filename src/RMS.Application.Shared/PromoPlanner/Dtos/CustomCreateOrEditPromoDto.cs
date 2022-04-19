using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RMS.PromoPlanner.Dtos
{
    public sealed class CustomCreateOrEditPromoDto
    {
        public CreateOrEditPromoDto Promo { get; set; }
        public IEnumerable<long> SelectedCountryIds { get; set; }
    }
}