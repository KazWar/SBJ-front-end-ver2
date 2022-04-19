using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.MakitaSerialNumber.Exporting;
using RMS.SBJ.MakitaSerialNumber.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.MakitaSerialNumber
{
    [AbpAuthorize(AppPermissions.Pages_MakitaSerialNumbers)]
    public class MakitaSerialNumbersAppService : RMSAppServiceBase, IMakitaSerialNumbersAppService
    {
        private readonly IRepository<MakitaSerialNumber, long> _makitaSerialNumberRepository;
        private readonly IMakitaSerialNumbersExcelExporter _makitaSerialNumbersExcelExporter;

        public MakitaSerialNumbersAppService(IRepository<MakitaSerialNumber, long> makitaSerialNumberRepository, IMakitaSerialNumbersExcelExporter makitaSerialNumbersExcelExporter)
        {
            _makitaSerialNumberRepository = makitaSerialNumberRepository;
            _makitaSerialNumbersExcelExporter = makitaSerialNumbersExcelExporter;

        }

        public async Task<PagedResultDto<GetMakitaSerialNumberForViewDto>> GetAll(GetAllMakitaSerialNumbersInput input)
        {

            var filteredMakitaSerialNumbers = _makitaSerialNumberRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ProductCode.Contains(input.Filter) || e.SerialNumber.Contains(input.Filter) || e.RetailerExternalCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCodeFilter), e => e.ProductCode == input.ProductCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SerialNumberFilter), e => e.SerialNumber == input.SerialNumberFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RetailerExternalCodeFilter), e => e.RetailerExternalCode == input.RetailerExternalCodeFilter);

            var pagedAndFilteredMakitaSerialNumbers = filteredMakitaSerialNumbers
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var makitaSerialNumbers = from o in pagedAndFilteredMakitaSerialNumbers
                                      select new GetMakitaSerialNumberForViewDto()
                                      {
                                          MakitaSerialNumber = new MakitaSerialNumberDto
                                          {
                                              ProductCode = o.ProductCode,
                                              SerialNumber = o.SerialNumber,
                                              RetailerExternalCode = o.RetailerExternalCode,
                                              Id = o.Id
                                          }
                                      };

            var totalCount = await filteredMakitaSerialNumbers.CountAsync();

            return new PagedResultDto<GetMakitaSerialNumberForViewDto>(
                totalCount,
                await makitaSerialNumbers.ToListAsync()
            );
        }

        public async Task<GetMakitaSerialNumberForViewDto> GetMakitaSerialNumberForView(long id)
        {
            var makitaSerialNumber = await _makitaSerialNumberRepository.GetAsync(id);

            var output = new GetMakitaSerialNumberForViewDto { MakitaSerialNumber = ObjectMapper.Map<MakitaSerialNumberDto>(makitaSerialNumber) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_MakitaSerialNumbers_Edit)]
        public async Task<GetMakitaSerialNumberForEditOutput> GetMakitaSerialNumberForEdit(EntityDto<long> input)
        {
            var makitaSerialNumber = await _makitaSerialNumberRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetMakitaSerialNumberForEditOutput { MakitaSerialNumber = ObjectMapper.Map<CreateOrEditMakitaSerialNumberDto>(makitaSerialNumber) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditMakitaSerialNumberDto input)
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

        [AbpAuthorize(AppPermissions.Pages_MakitaSerialNumbers_Create)]
        protected virtual async Task Create(CreateOrEditMakitaSerialNumberDto input)
        {
            var makitaSerialNumber = ObjectMapper.Map<MakitaSerialNumber>(input);

            if (AbpSession.TenantId != null)
            {
                makitaSerialNumber.TenantId = (int?)AbpSession.TenantId;
            }

            await _makitaSerialNumberRepository.InsertAsync(makitaSerialNumber);
        }

        [AbpAuthorize(AppPermissions.Pages_MakitaSerialNumbers_Edit)]
        protected virtual async Task Update(CreateOrEditMakitaSerialNumberDto input)
        {
            var makitaSerialNumber = await _makitaSerialNumberRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, makitaSerialNumber);
        }

        [AbpAuthorize(AppPermissions.Pages_MakitaSerialNumbers_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _makitaSerialNumberRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetMakitaSerialNumbersToExcel(GetAllMakitaSerialNumbersForExcelInput input)
        {

            var filteredMakitaSerialNumbers = _makitaSerialNumberRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ProductCode.Contains(input.Filter) || e.SerialNumber.Contains(input.Filter) || e.RetailerExternalCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCodeFilter), e => e.ProductCode == input.ProductCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SerialNumberFilter), e => e.SerialNumber == input.SerialNumberFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RetailerExternalCodeFilter), e => e.RetailerExternalCode == input.RetailerExternalCodeFilter);

            var query = (from o in filteredMakitaSerialNumbers
                         select new GetMakitaSerialNumberForViewDto()
                         {
                             MakitaSerialNumber = new MakitaSerialNumberDto
                             {
                                 ProductCode = o.ProductCode,
                                 SerialNumber = o.SerialNumber,
                                 RetailerExternalCode = o.RetailerExternalCode,
                                 Id = o.Id
                             }
                         });

            var makitaSerialNumberListDtos = await query.ToListAsync();

            return _makitaSerialNumbersExcelExporter.ExportToFile(makitaSerialNumberListDtos);
        }

    }
}