using RMS.SBJ.Forms;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.RegistrationFormFields.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using RMS.SBJ.RegistrationFields;

namespace RMS.SBJ.RegistrationFormFields
{
	[AbpAuthorize(AppPermissions.Pages_RegistrationFormFields)]
    public class RegistrationFormFieldsAppService : RMSAppServiceBase, IRegistrationFormFieldsAppService
    {
		 private readonly IRepository<RegistrationField, long> _registrationFormFieldRepository;
		 private readonly IRepository<FormField,long> _lookup_formFieldRepository;
		 

		  public RegistrationFormFieldsAppService(IRepository<RegistrationField, long> registrationFormFieldRepository , IRepository<FormField, long> lookup_formFieldRepository) 
		  {
			_registrationFormFieldRepository = registrationFormFieldRepository;
			_lookup_formFieldRepository = lookup_formFieldRepository;
		
		  }

		 public async Task<PagedResultDto<GetRegistrationFormFieldForViewDto>> GetAll(GetAllRegistrationFormFieldsInput input)
         {
			
			var filteredRegistrationFormFields = _registrationFormFieldRepository.GetAll()
						.Include( e => e.FormFieldFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.FormFieldDescriptionFilter), e => e.FormFieldFk != null && e.FormFieldFk.Description == input.FormFieldDescriptionFilter);

			var pagedAndFilteredRegistrationFormFields = filteredRegistrationFormFields
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var registrationFormFields = from o in pagedAndFilteredRegistrationFormFields
                         join o1 in _lookup_formFieldRepository.GetAll() on o.FormFieldId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetRegistrationFormFieldForViewDto() {
							RegistrationFormField = new RegistrationFormFieldDto
							{
                                Description = o.Description,
                                Id = o.Id
							},
                         	FormFieldDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString()
						};

            var totalCount = await filteredRegistrationFormFields.CountAsync();

            return new PagedResultDto<GetRegistrationFormFieldForViewDto>(
                totalCount,
                await registrationFormFields.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_RegistrationFormFields_Edit)]
		 public async Task<GetRegistrationFormFieldForEditOutput> GetRegistrationFormFieldForEdit(EntityDto<long> input)
         {
            var registrationFormField = await _registrationFormFieldRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetRegistrationFormFieldForEditOutput {RegistrationFormField = ObjectMapper.Map<CreateOrEditRegistrationFormFieldDto>(registrationFormField)};

		    if (output.RegistrationFormField.FormFieldId != null)
            {
                var _lookupFormField = await _lookup_formFieldRepository.FirstOrDefaultAsync((long)output.RegistrationFormField.FormFieldId);
                output.FormFieldDescription = _lookupFormField?.Description?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditRegistrationFormFieldDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_RegistrationFormFields_Create)]
		 protected virtual async Task Create(CreateOrEditRegistrationFormFieldDto input)
         {
            var registrationFormField = ObjectMapper.Map<RegistrationField>(input);

			
			if (AbpSession.TenantId != null)
			{
				registrationFormField.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _registrationFormFieldRepository.InsertAsync(registrationFormField);
         }

		 [AbpAuthorize(AppPermissions.Pages_RegistrationFormFields_Edit)]
		 protected virtual async Task Update(CreateOrEditRegistrationFormFieldDto input)
         {
            var registrationFormField = await _registrationFormFieldRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, registrationFormField);
         }

		 [AbpAuthorize(AppPermissions.Pages_RegistrationFormFields_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _registrationFormFieldRepository.DeleteAsync(input.Id);
         } 

		[AbpAuthorize(AppPermissions.Pages_RegistrationFormFields)]
         public async Task<PagedResultDto<RegistrationFormFieldFormFieldLookupTableDto>> GetAllFormFieldForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_formFieldRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Description != null && e.Description.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var formFieldList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<RegistrationFormFieldFormFieldLookupTableDto>();
			foreach(var formField in formFieldList){
				lookupTableDtoList.Add(new RegistrationFormFieldFormFieldLookupTableDto
				{
					Id = formField.Id,
					DisplayName = formField.Description?.ToString()
				});
			}

            return new PagedResultDto<RegistrationFormFieldFormFieldLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}