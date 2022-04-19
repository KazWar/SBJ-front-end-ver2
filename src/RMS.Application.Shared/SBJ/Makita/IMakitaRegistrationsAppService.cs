using System.Threading.Tasks;
using Abp.Application.Services;
using RMS.SBJ.Makita.Dtos;

namespace RMS.SBJ.Makita
{
    public interface IMakitaRegistrationsAppService : IApplicationService
    {
        Task<object> Update(string tenantSettings, string rMSBlobStorage, MakitaRegistrationsDto makitaRegistrationsModel);

        Task<object> Create(string tenantSettings, string rMSBlobStorage, MakitaRegistrationsDto makitaRegistrationsModel);

        Task<object> GetRegistrationStatus(long registrationId);
    }
}
