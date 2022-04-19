using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.MakitaBaseModelSerial.Dtos;
using RMS.Dto;

namespace RMS.SBJ.MakitaBaseModelSerial
{
    public interface IMakitaBaseModelSerialsAppService : IApplicationService
    {
        Task<PagedResultDto<GetMakitaBaseModelSerialForViewDto>> GetAll(GetAllMakitaBaseModelSerialsInput input);

        Task<GetMakitaBaseModelSerialForViewDto> GetMakitaBaseModelSerialForView(long id);

        Task<GetMakitaBaseModelSerialForEditOutput> GetMakitaBaseModelSerialForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditMakitaBaseModelSerialDto input);

        Task Delete(EntityDto<long> input);

    }
}