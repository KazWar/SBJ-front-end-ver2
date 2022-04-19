using RMS.SBJ.Forms;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.PurchaseRegistrationFields.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.PurchaseRegistrationFields
{
	[AbpAuthorize(AppPermissions.Pages_PurchaseRegistrationFields)]
    public class PurchaseRegistrationFieldsAppService : RMSAppServiceBase, IPurchaseRegistrationFieldsAppService
    {
		 private readonly IRepository<PurchaseRegistrationField, long> _purchaseRegistrationFieldRepository;
		 private readonly IRepository<FormField,long> _lookup_formFieldRepository;
		 

		  public PurchaseRegistrationFieldsAppService(IRepository<PurchaseRegistrationField, long> purchaseRegistrationFieldRepository , IRepository<FormField, long> lookup_formFieldRepository) 
		  {
			_purchaseRegistrationFieldRepository = purchaseRegistrationFieldRepository;
			_lookup_formFieldRepository = lookup_formFieldRepository;
		
		  }

		 public async Task<PagedResultDto<GetPurchaseRegistrationFieldForViewDto>> GetAll(GetAllPurchaseRegistrationFieldsInput input)
         {
			
			var filteredPurchaseRegistrationFields = _purchaseRegistrationFieldRepository.GetAll()
						.Include( e => e.FormFieldFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.FormFieldDescriptionFilter), e => e.FormFieldFk != null && e.FormFieldFk.Description == input.FormFieldDescriptionFilter);

			var pagedAndFilteredPurchaseRegistrationFields = filteredPurchaseRegistrationFields
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var purchaseRegistrationFields = from o in pagedAndFilteredPurchaseRegistrationFields
                         join o1 in _lookup_formFieldRepository.GetAll() on o.FormFieldId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetPurchaseRegistrationFieldForViewDto() {
							PurchaseRegistrationField = new PurchaseRegistrationFieldDto
							{
                                Description = o.Description,
                                Id = o.Id
							},
                         	FormFieldDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString()
						};

            var totalCount = await filteredPurchaseRegistrationFields.CountAsync();

            return new PagedResultDto<GetPurchaseRegistrationFieldForViewDto>(
                totalCount,
                await purchaseRegistrationFields.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_PurchaseRegistrationFields_Edit)]
		 public async Task<GetPurchaseRegistrationFieldForEditOutput> GetPurchaseRegistrationFieldForEdit(EntityDto<long> input)
         {
            var purchaseRegistrationField = await _purchaseRegistrationFieldRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetPurchaseRegistrationFieldForEditOutput {PurchaseRegistrationField = ObjectMapper.Map<CreateOrEditPurchaseRegistrationFieldDto>(purchaseRegistrationField)};

		    if (output.PurchaseRegistrationField.FormFieldId != null)
            {
                var _lookupFormField = await _lookup_formFieldRepository.FirstOrDefaultAsync((long)output.PurchaseRegistrationField.FormFieldId);
                output.FormFieldDescription = _lookupFormField?.Description?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditPurchaseRegistrationFieldDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_PurchaseRegistrationFields_Create)]
		 protected virtual async Task Create(CreateOrEditPurchaseRegistrationFieldDto input)
         {
            var purchaseRegistrationField = ObjectMapper.Map<PurchaseRegistrationField>(input);

			
			if (AbpSession.TenantId != null)
			{
				purchaseRegistrationField.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _purchaseRegistrationFieldRepository.InsertAsync(purchaseRegistrationField);
         }

		 [AbpAuthorize(AppPermissions.Pages_PurchaseRegistrationFields_Edit)]
		 protected virtual async Task Update(CreateOrEditPurchaseRegistrationFieldDto input)
         {
            var purchaseRegistrationField = await _purchaseRegistrationFieldRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, purchaseRegistrationField);
         }

		 [AbpAuthorize(AppPermissions.Pages_PurchaseRegistrationFields_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _purchaseRegistrationFieldRepository.DeleteAsync(input.Id);
         } 

		[AbpAuthorize(AppPermissions.Pages_PurchaseRegistrationFields)]
         public async Task<PagedResultDto<PurchaseRegistrationFieldFormFieldLookupTableDto>> GetAllFormFieldForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_formFieldRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Description != null && e.Description.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var formFieldList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<PurchaseRegistrationFieldFormFieldLookupTableDto>();
			foreach(var formField in formFieldList){
				lookupTableDtoList.Add(new PurchaseRegistrationFieldFormFieldLookupTableDto
				{
					Id = formField.Id,
					DisplayName = formField.Description?.ToString()
				});
			}

            return new PagedResultDto<PurchaseRegistrationFieldFormFieldLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}