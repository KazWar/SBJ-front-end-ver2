using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.MakitaBaseModelSerial.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using RMS.Storage;

namespace RMS.SBJ.MakitaBaseModelSerial
{
    [AbpAuthorize(AppPermissions.Pages_MakitaBaseModelSerials)]
    public class MakitaBaseModelSerialsAppService : RMSAppServiceBase, IMakitaBaseModelSerialsAppService
    {
        private readonly IRepository<MakitaBaseModelSerial, long> _makitaBaseModelSerialRepository;

        public MakitaBaseModelSerialsAppService(IRepository<MakitaBaseModelSerial, long> makitaBaseModelSerialRepository)
        {
            _makitaBaseModelSerialRepository = makitaBaseModelSerialRepository;

        }

        public async Task<PagedResultDto<GetMakitaBaseModelSerialForViewDto>> GetAll(GetAllMakitaBaseModelSerialsInput input)
        {

            var filteredMakitaBaseModelSerials = _makitaBaseModelSerialRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ItemNumber.Contains(input.Filter) || e.BasisModel.Contains(input.Filter));

            var pagedAndFilteredMakitaBaseModelSerials = filteredMakitaBaseModelSerials
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var makitaBaseModelSerials = from o in pagedAndFilteredMakitaBaseModelSerials
                                         select new
                                         {

                                             Id = o.Id
                                         };

            var totalCount = await filteredMakitaBaseModelSerials.CountAsync();

            var dbList = await makitaBaseModelSerials.ToListAsync();
            var results = new List<GetMakitaBaseModelSerialForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetMakitaBaseModelSerialForViewDto()
                {
                    MakitaBaseModelSerial = new MakitaBaseModelSerialDto
                    {

                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetMakitaBaseModelSerialForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetMakitaBaseModelSerialForViewDto> GetMakitaBaseModelSerialForView(long id)
        {
            var makitaBaseModelSerial = await _makitaBaseModelSerialRepository.GetAsync(id);

            var output = new GetMakitaBaseModelSerialForViewDto { MakitaBaseModelSerial = ObjectMapper.Map<MakitaBaseModelSerialDto>(makitaBaseModelSerial) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_MakitaBaseModelSerials_Edit)]
        public async Task<GetMakitaBaseModelSerialForEditOutput> GetMakitaBaseModelSerialForEdit(EntityDto<long> input)
        {
            var makitaBaseModelSerial = await _makitaBaseModelSerialRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetMakitaBaseModelSerialForEditOutput { MakitaBaseModelSerial = ObjectMapper.Map<CreateOrEditMakitaBaseModelSerialDto>(makitaBaseModelSerial) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditMakitaBaseModelSerialDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_MakitaBaseModelSerials_Create)]
        protected virtual async Task Create(CreateOrEditMakitaBaseModelSerialDto input)
        {
            var makitaBaseModelSerial = ObjectMapper.Map<MakitaBaseModelSerial>(input);

            if (AbpSession.TenantId != null)
            {
                makitaBaseModelSerial.TenantId = (int?)AbpSession.TenantId;
            }

            await _makitaBaseModelSerialRepository.InsertAsync(makitaBaseModelSerial);

        }

        [AbpAuthorize(AppPermissions.Pages_MakitaBaseModelSerials_Edit)]
        protected virtual async Task Update(CreateOrEditMakitaBaseModelSerialDto input)
        {
            var makitaBaseModelSerial = await _makitaBaseModelSerialRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, makitaBaseModelSerial);

        }

        [AbpAuthorize(AppPermissions.Pages_MakitaBaseModelSerials_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _makitaBaseModelSerialRepository.DeleteAsync(input.Id);
        }

    }
}