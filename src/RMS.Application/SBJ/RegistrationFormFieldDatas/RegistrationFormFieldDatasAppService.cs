using RMS.SBJ.RegistrationFormFields;
using RMS.SBJ.Registrations;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.RegistrationFormFieldDatas.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using RMS.SBJ.RegistrationFields;

namespace RMS.SBJ.RegistrationFormFieldDatas
{
	[AbpAuthorize(AppPermissions.Pages_RegistrationFormFieldDatas)]
    public class RegistrationFormFieldDatasAppService : RMSAppServiceBase, IRegistrationFormFieldDatasAppService
    {
		 private readonly IRepository<RegistrationFieldData, long> _registrationFormFieldDataRepository;
		 private readonly IRepository<RegistrationField,long> _lookup_registrationFormFieldRepository;
		 private readonly IRepository<Registration,long> _lookup_registrationRepository;
		 

		  public RegistrationFormFieldDatasAppService(IRepository<RegistrationFieldData, long> registrationFormFieldDataRepository , IRepository<RegistrationField, long> lookup_registrationFormFieldRepository, IRepository<Registration, long> lookup_registrationRepository) 
		  {
			_registrationFormFieldDataRepository = registrationFormFieldDataRepository;
			_lookup_registrationFormFieldRepository = lookup_registrationFormFieldRepository;
		    _lookup_registrationRepository = lookup_registrationRepository;	
		  }

		 public async Task<PagedResultDto<GetRegistrationFormFieldDataForViewDto>> GetAll(GetAllRegistrationFormFieldDatasInput input)
         {
			
			var filteredRegistrationFormFieldDatas = _registrationFormFieldDataRepository.GetAll()
						.Include( e => e.RegistrationFieldFk)
						.Include( e => e.RegistrationFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Value.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.ValueFilter),  e => e.Value == input.ValueFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.RegistrationFormFieldDescriptionFilter), e => e.RegistrationFieldFk != null && e.RegistrationFieldFk.Description == input.RegistrationFormFieldDescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.RegistrationFirstNameFilter), e => e.RegistrationFk != null && e.RegistrationFk.FirstName == input.RegistrationFirstNameFilter);

			var pagedAndFilteredRegistrationFormFieldDatas = filteredRegistrationFormFieldDatas
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var registrationFormFieldDatas = from o in pagedAndFilteredRegistrationFormFieldDatas
                         join o1 in _lookup_registrationFormFieldRepository.GetAll() on o.RegistrationFieldId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_registrationRepository.GetAll() on o.RegistrationId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetRegistrationFormFieldDataForViewDto() {
							RegistrationFormFieldData = new RegistrationFormFieldDataDto
							{
                                Value = o.Value,
                                Id = o.Id
							},
                         	RegistrationFormFieldDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString(),
                         	RegistrationFirstName = s2 == null || s2.FirstName == null ? "" : s2.FirstName.ToString()
						};

            var totalCount = await filteredRegistrationFormFieldDatas.CountAsync();

            return new PagedResultDto<GetRegistrationFormFieldDataForViewDto>(
                totalCount,
                await registrationFormFieldDatas.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_RegistrationFormFieldDatas_Edit)]
		 public async Task<GetRegistrationFormFieldDataForEditOutput> GetRegistrationFormFieldDataForEdit(EntityDto<long> input)
         {
            var registrationFormFieldData = await _registrationFormFieldDataRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetRegistrationFormFieldDataForEditOutput {RegistrationFormFieldData = ObjectMapper.Map<CreateOrEditRegistrationFormFieldDataDto>(registrationFormFieldData)};

		    if (output.RegistrationFormFieldData.RegistrationFormFieldId != null)
            {
                var _lookupRegistrationFormField = await _lookup_registrationFormFieldRepository.FirstOrDefaultAsync((long)output.RegistrationFormFieldData.RegistrationFormFieldId);
                output.RegistrationFormFieldDescription = _lookupRegistrationFormField?.Description?.ToString();
            }

		    if (output.RegistrationFormFieldData.RegistrationId != null)
            {
                var _lookupRegistration = await _lookup_registrationRepository.FirstOrDefaultAsync((long)output.RegistrationFormFieldData.RegistrationId);
                output.RegistrationFirstName = _lookupRegistration?.FirstName?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditRegistrationFormFieldDataDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_RegistrationFormFieldDatas_Create)]
		 protected virtual async Task Create(CreateOrEditRegistrationFormFieldDataDto input)
         {
            var registrationFormFieldData = ObjectMapper.Map<RegistrationFieldData>(input);

			
			if (AbpSession.TenantId != null)
			{
				registrationFormFieldData.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _registrationFormFieldDataRepository.InsertAsync(registrationFormFieldData);
         }

		 [AbpAuthorize(AppPermissions.Pages_RegistrationFormFieldDatas_Edit)]
		 protected virtual async Task Update(CreateOrEditRegistrationFormFieldDataDto input)
         {
            var registrationFormFieldData = await _registrationFormFieldDataRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, registrationFormFieldData);
         }

		 [AbpAuthorize(AppPermissions.Pages_RegistrationFormFieldDatas_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _registrationFormFieldDataRepository.DeleteAsync(input.Id);
         } 

		[AbpAuthorize(AppPermissions.Pages_RegistrationFormFieldDatas)]
         public async Task<PagedResultDto<RegistrationFormFieldDataRegistrationFormFieldLookupTableDto>> GetAllRegistrationFormFieldForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_registrationFormFieldRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Description != null && e.Description.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var registrationFormFieldList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<RegistrationFormFieldDataRegistrationFormFieldLookupTableDto>();
			foreach(var registrationFormField in registrationFormFieldList){
				lookupTableDtoList.Add(new RegistrationFormFieldDataRegistrationFormFieldLookupTableDto
				{
					Id = registrationFormField.Id,
					DisplayName = registrationFormField.Description?.ToString()
				});
			}

            return new PagedResultDto<RegistrationFormFieldDataRegistrationFormFieldLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_RegistrationFormFieldDatas)]
         public async Task<PagedResultDto<RegistrationFormFieldDataRegistrationLookupTableDto>> GetAllRegistrationForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_registrationRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.FirstName != null && e.FirstName.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var registrationList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<RegistrationFormFieldDataRegistrationLookupTableDto>();
			foreach(var registration in registrationList){
				lookupTableDtoList.Add(new RegistrationFormFieldDataRegistrationLookupTableDto
				{
					Id = registration.Id,
					DisplayName = registration.FirstName?.ToString()
				});
			}

            return new PagedResultDto<RegistrationFormFieldDataRegistrationLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}