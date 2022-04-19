using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RMS.MultiTenancy.HostDashboard.Dto;

namespace RMS.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}