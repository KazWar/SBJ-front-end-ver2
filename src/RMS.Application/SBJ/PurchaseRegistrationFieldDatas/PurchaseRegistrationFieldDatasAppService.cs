using RMS.SBJ.PurchaseRegistrationFormFields;
using RMS.SBJ.PurchaseRegistrations;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.PurchaseRegistrationFieldDatas.Dtos;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.PurchaseRegistrationFieldDatas
{
    [AbpAuthorize(AppPermissions.Pages_PurchaseRegistrationFieldDatas)]
    public class PurchaseRegistrationFieldDatasAppService : RMSAppServiceBase, IPurchaseRegistrationFieldDatasAppService
    {
        private readonly IRepository<PurchaseRegistrationFieldData, long> _purchaseRegistrationFieldDataRepository;
        private readonly IRepository<PurchaseRegistrationFormField, long> _lookup_purchaseRegistrationFormFieldRepository;
        private readonly IRepository<PurchaseRegistration, long> _lookup_purchaseRegistrationRepository;

        public PurchaseRegistrationFieldDatasAppService(IRepository<PurchaseRegistrationFieldData, long> purchaseRegistrationFieldDataRepository, IRepository<PurchaseRegistrationFormField, long> lookup_purchaseRegistrationFormFieldRepository, IRepository<PurchaseRegistration, long> lookup_purchaseRegistrationRepository)
        {
            _purchaseRegistrationFieldDataRepository = purchaseRegistrationFieldDataRepository;
            _lookup_purchaseRegistrationFormFieldRepository = lookup_purchaseRegistrationFormFieldRepository;
            _lookup_purchaseRegistrationRepository = lookup_purchaseRegistrationRepository;

        }

        public async Task<PagedResultDto<GetPurchaseRegistrationFieldDataForViewDto>> GetAll(GetAllPurchaseRegistrationFieldDatasInput input)
        {
            var filteredPurchaseRegistrationFieldDatas = _purchaseRegistrationFieldDataRepository.GetAll()
                        .Include(e => e.PurchaseRegistrationFieldFk)
                        .Include(e => e.PurchaseRegistrationFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PurchaseRegistrationFormFieldDescriptionFilter), e => e.PurchaseRegistrationFieldFk != null && e.PurchaseRegistrationFieldFk.Description == input.PurchaseRegistrationFormFieldDescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PurchaseRegistrationFormFieldDescriptionFilter), e => e.PurchaseRegistrationFieldFk != null && e.PurchaseRegistrationFieldFk.Description == input.PurchaseRegistrationFormFieldDescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PurchaseRegistrationInvoiceImageFilter), e => e.PurchaseRegistrationFk != null && e.PurchaseRegistrationFk.InvoiceImage == input.PurchaseRegistrationInvoiceImageFilter);

            var pagedAndFilteredPurchaseRegistrationFieldDatas = filteredPurchaseRegistrationFieldDatas
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var purchaseRegistrationFieldDatas =
                from o in pagedAndFilteredPurchaseRegistrationFieldDatas
                join o1 in _lookup_purchaseRegistrationFormFieldRepository.GetAll() on o.PurchaseRegistrationFieldId equals o1.Id into j1

                from s1 in j1.DefaultIfEmpty()

                join o2 in _lookup_purchaseRegistrationRepository.GetAll() on o.PurchaseRegistrationId equals o2.Id into j2
                from s2 in j2.DefaultIfEmpty()

                select new GetPurchaseRegistrationFieldDataForViewDto()
                {
                    PurchaseRegistrationFieldData = new PurchaseRegistrationFieldDataDto
                    {
                        Id = o.Id
                    },
                    PurchaseRegistrationFormFieldDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString(),
                    PurchaseRegistrationInvoiceImage = s2 == null || s2.InvoiceImage == null ? "" : s2.InvoiceImage.ToString()
                };

            var totalCount = await filteredPurchaseRegistrationFieldDatas.CountAsync();

            return new PagedResultDto<GetPurchaseRegistrationFieldDataForViewDto>(
                totalCount,
                await purchaseRegistrationFieldDatas.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseRegistrationFieldDatas_Edit)]
        public async Task<GetPurchaseRegistrationFieldDataForEditOutput> GetPurchaseRegistrationFieldDataForEdit(EntityDto<long> input)
        {
            var purchaseRegistrationFieldData = await _purchaseRegistrationFieldDataRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPurchaseRegistrationFieldDataForEditOutput { PurchaseRegistrationFieldData = ObjectMapper.Map<CreateOrEditPurchaseRegistrationFieldDataDto>(purchaseRegistrationFieldData) };

            if (output.PurchaseRegistrationFieldData.PurchaseRegistrationFieldId != null)
            {
                var _lookupPurchaseRegistrationFormField = await _lookup_purchaseRegistrationFormFieldRepository.FirstOrDefaultAsync((long)output.PurchaseRegistrationFieldData.PurchaseRegistrationFieldId);
                output.PurchaseRegistrationFormFieldDescription = _lookupPurchaseRegistrationFormField?.Description?.ToString();
            }

            if (output.PurchaseRegistrationFieldData.PurchaseRegistrationId != null)
            {
                var _lookupPurchaseRegistration = await _lookup_purchaseRegistrationRepository.FirstOrDefaultAsync((long)output.PurchaseRegistrationFieldData.PurchaseRegistrationId);
                output.PurchaseRegistrationInvoiceImage = _lookupPurchaseRegistration?.InvoiceImage?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPurchaseRegistrationFieldDataDto input)
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

        [AbpAuthorize(AppPermissions.Pages_PurchaseRegistrationFieldDatas_Create)]
        protected virtual async Task Create(CreateOrEditPurchaseRegistrationFieldDataDto input)
        {
            var purchaseRegistrationFieldData = ObjectMapper.Map<PurchaseRegistrationFieldData>(input);

            if (AbpSession.TenantId != null)
            {
                purchaseRegistrationFieldData.TenantId = (int?)AbpSession.TenantId;
            }

            await _purchaseRegistrationFieldDataRepository.InsertAsync(purchaseRegistrationFieldData);
        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseRegistrationFieldDatas_Edit)]
        protected virtual async Task Update(CreateOrEditPurchaseRegistrationFieldDataDto input)
        {
            var purchaseRegistrationFieldData = await _purchaseRegistrationFieldDataRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, purchaseRegistrationFieldData);
        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseRegistrationFieldDatas_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _purchaseRegistrationFieldDataRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseRegistrationFieldDatas)]
        public async Task<PagedResultDto<PurchaseRegistrationFieldDataPurchaseRegistrationFormFieldLookupTableDto>> GetAllPurchaseRegistrationFormFieldForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_purchaseRegistrationFormFieldRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Description != null && e.Description.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var purchaseRegistrationFormFieldList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<PurchaseRegistrationFieldDataPurchaseRegistrationFormFieldLookupTableDto>();
            foreach (var purchaseRegistrationFormField in purchaseRegistrationFormFieldList)
            {
                lookupTableDtoList.Add(new PurchaseRegistrationFieldDataPurchaseRegistrationFormFieldLookupTableDto
                {
                    Id = purchaseRegistrationFormField.Id,
                    DisplayName = purchaseRegistrationFormField.Description?.ToString()
                });
            }

            return new PagedResultDto<PurchaseRegistrationFieldDataPurchaseRegistrationFormFieldLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_PurchaseRegistrationFieldDatas)]
        public async Task<PagedResultDto<PurchaseRegistrationFieldDataPurchaseRegistrationLookupTableDto>> GetAllPurchaseRegistrationForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_purchaseRegistrationRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.InvoiceImage != null && e.InvoiceImage.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var purchaseRegistrationList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<PurchaseRegistrationFieldDataPurchaseRegistrationLookupTableDto>();
            foreach (var purchaseRegistration in purchaseRegistrationList)
            {
                lookupTableDtoList.Add(new PurchaseRegistrationFieldDataPurchaseRegistrationLookupTableDto
                {
                    Id = purchaseRegistration.Id,
                    DisplayName = purchaseRegistration.InvoiceImage?.ToString()
                });
            }

            return new PagedResultDto<PurchaseRegistrationFieldDataPurchaseRegistrationLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}