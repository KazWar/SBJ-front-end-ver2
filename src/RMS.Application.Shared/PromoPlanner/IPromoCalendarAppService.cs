using Abp.Application.Services;
using RMS.PromoPlanner.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.PromoPlanner
{
    public interface IPromoCalendarAppService : IApplicationService
    {
        Task<List<GetPromoCalendarEventsDto>> GetAllEvents(DateTime Start, DateTime End, GetAllPromosInput PromoFilter);
    }
}
