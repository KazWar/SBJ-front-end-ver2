using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.Dto;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.SBJ.Makita.Dtos;

namespace RMS.SBJ.Makita
{
    public interface IMakitaApiAppService : IApplicationService
    {
        Task<object> MakitaRegistrationApproved(long registrationId);

    }
}
