using RMS.SBJ.Forms;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.PurchaseRegistrationFormFields.Exporting;
using RMS.SBJ.PurchaseRegistrationFormFields.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.PurchaseRegistrationFormFields
{
	[AbpAuthorize(AppPermissions.Pages_PurchaseRegistrationFormFields)]
    public class PurchaseRegistrationFormFieldsAppService : RMSAppServiceBase, IPurchaseRegistrationFormFieldsAppService
    {
		 private readonly IRepository<PurchaseRegistrationFormField, long> _purchaseRegistrationFormFieldRepository;
		 private readonly IPurchaseRegistrationFormFieldsExcelExporter _purchaseRegistrationFormFieldsExcelExporter;
		 private readonly IRepository<FormField,long> _lookup_formFieldRepository;
		 

		  public PurchaseRegistrationFormFieldsAppService(IRepository<PurchaseRegistrationFormField, long> purchaseRegistrationFormFieldRepository, IPurchaseRegistrationFormFieldsExcelExporter purchaseRegistrationFormFieldsExcelExporter , IRepository<FormField, long> lookup_formFieldRepository) 
		  {
			_purchaseRegistrationFormFieldRepository = purchaseRegistrationFormFieldRepository;
			_purchaseRegistrationFormFieldsExcelExporter = purchaseRegistrationFormFieldsExcelExporter;
			_lookup_formFieldRepository = lookup_formFieldRepository;
		
		  }

		 public async Task<PagedResultDto<GetPurchaseRegistrationFormFieldForViewDto>> GetAll(GetAllPurchaseRegistrationFormFieldsInput input)
         {
			
			var filteredPurchaseRegistrationFormFields = _purchaseRegistrationFormFieldRepository.GetAll()
						.Include( e => e.FormFieldFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.FormFieldDescriptionFilter), e => e.FormFieldFk != null && e.FormFieldFk.Description == input.FormFieldDescriptionFilter);

			var pagedAndFilteredPurchaseRegistrationFormFields = filteredPurchaseRegistrationFormFields
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var purchaseRegistrationFormFields = from o in pagedAndFilteredPurchaseRegistrationFormFields
                         join o1 in _lookup_formFieldRepository.GetAll() on o.FormFieldId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetPurchaseRegistrationFormFieldForViewDto() {
							PurchaseRegistrationFormField = new PurchaseRegistrationFormFieldDto
							{
                                Description = o.Description,
                                Id = o.Id
							},
                         	FormFieldDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString()
						};

            var totalCount = await filteredPurchaseRegistrationFormFields.CountAsync();

            return new PagedResultDto<GetPurchaseRegistrationFormFieldForViewDto>(
                totalCount,
                await purchaseRegistrationFormFields.ToListAsync()
            );
         }
		 
		 public async Task<GetPurchaseRegistrationFormFieldForViewDto> GetPurchaseRegistrationFormFieldForView(long id)
         {
            var purchaseRegistrationFormField = await _purchaseRegistrationFormFieldRepository.GetAsync(id);

            var output = new GetPurchaseRegistrationFormFieldForViewDto { PurchaseRegistrationFormField = ObjectMapper.Map<PurchaseRegistrationFormFieldDto>(purchaseRegistrationFormField) };

		    if (output.PurchaseRegistrationFormField.FormFieldId != null)
            {
                var _lookupFormField = await _lookup_formFieldRepository.FirstOrDefaultAsync((long)output.PurchaseRegistrationFormField.FormFieldId);
                output.FormFieldDescription = _lookupFormField?.Description?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_PurchaseRegistrationFormFields_Edit)]
		 public async Task<GetPurchaseRegistrationFormFieldForEditOutput> GetPurchaseRegistrationFormFieldForEdit(EntityDto<long> input)
         {
            var purchaseRegistrationFormField = await _purchaseRegistrationFormFieldRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetPurchaseRegistrationFormFieldForEditOutput {PurchaseRegistrationFormField = ObjectMapper.Map<CreateOrEditPurchaseRegistrationFormFieldDto>(purchaseRegistrationFormField)};

		    if (output.PurchaseRegistrationFormField.FormFieldId != null)
            {
                var _lookupFormField = await _lookup_formFieldRepository.FirstOrDefaultAsync((long)output.PurchaseRegistrationFormField.FormFieldId);
                output.FormFieldDescription = _lookupFormField?.Description?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditPurchaseRegistrationFormFieldDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_PurchaseRegistrationFormFields_Create)]
		 protected virtual async Task Create(CreateOrEditPurchaseRegistrationFormFieldDto input)
         {
            var purchaseRegistrationFormField = ObjectMapper.Map<PurchaseRegistrationFormField>(input);

			
			if (AbpSession.TenantId != null)
			{
				purchaseRegistrationFormField.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _purchaseRegistrationFormFieldRepository.InsertAsync(purchaseRegistrationFormField);
         }

		 [AbpAuthorize(AppPermissions.Pages_PurchaseRegistrationFormFields_Edit)]
		 protected virtual async Task Update(CreateOrEditPurchaseRegistrationFormFieldDto input)
         {
            var purchaseRegistrationFormField = await _purchaseRegistrationFormFieldRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, purchaseRegistrationFormField);
         }

		 [AbpAuthorize(AppPermissions.Pages_PurchaseRegistrationFormFields_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _purchaseRegistrationFormFieldRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetPurchaseRegistrationFormFieldsToExcel(GetAllPurchaseRegistrationFormFieldsForExcelInput input)
         {
			
			var filteredPurchaseRegistrationFormFields = _purchaseRegistrationFormFieldRepository.GetAll()
						.Include( e => e.FormFieldFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.FormFieldDescriptionFilter), e => e.FormFieldFk != null && e.FormFieldFk.Description == input.FormFieldDescriptionFilter);

			var query = (from o in filteredPurchaseRegistrationFormFields
                         join o1 in _lookup_formFieldRepository.GetAll() on o.FormFieldId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetPurchaseRegistrationFormFieldForViewDto() { 
							PurchaseRegistrationFormField = new PurchaseRegistrationFormFieldDto
							{
                                Description = o.Description,
                                Id = o.Id
							},
                         	FormFieldDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString()
						 });


            var purchaseRegistrationFormFieldListDtos = await query.ToListAsync();

            return _purchaseRegistrationFormFieldsExcelExporter.ExportToFile(purchaseRegistrationFormFieldListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_PurchaseRegistrationFormFields)]
         public async Task<PagedResultDto<PurchaseRegistrationFormFieldFormFieldLookupTableDto>> GetAllFormFieldForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_formFieldRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Description != null && e.Description.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var formFieldList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<PurchaseRegistrationFormFieldFormFieldLookupTableDto>();
			foreach(var formField in formFieldList){
				lookupTableDtoList.Add(new PurchaseRegistrationFormFieldFormFieldLookupTableDto
				{
					Id = formField.Id,
					DisplayName = formField.Description?.ToString()
				});
			}

            return new PagedResultDto<PurchaseRegistrationFormFieldFormFieldLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}