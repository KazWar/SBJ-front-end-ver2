using RMS.SBJ.ActivationCodes;
using RMS.SBJ.Registrations;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.ActivationCodeRegistrations.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.ActivationCodeRegistrations
{
	[AbpAuthorize(AppPermissions.Pages_ActivationCodeRegistrations)]
    public class ActivationCodeRegistrationsAppService : RMSAppServiceBase, IActivationCodeRegistrationsAppService
    {
		 private readonly IRepository<ActivationCodeRegistration, long> _activationCodeRegistrationRepository;
		 private readonly IRepository<ActivationCode,long> _lookup_activationCodeRepository;
		 private readonly IRepository<Registration,long> _lookup_registrationRepository;
		 

		  public ActivationCodeRegistrationsAppService(IRepository<ActivationCodeRegistration, long> activationCodeRegistrationRepository , IRepository<ActivationCode, long> lookup_activationCodeRepository, IRepository<Registration, long> lookup_registrationRepository) 
		  {
			_activationCodeRegistrationRepository = activationCodeRegistrationRepository;
			_lookup_activationCodeRepository = lookup_activationCodeRepository;
		_lookup_registrationRepository = lookup_registrationRepository;
		
		  }

		 public async Task<PagedResultDto<GetActivationCodeRegistrationForViewDto>> GetAll(GetAllActivationCodeRegistrationsInput input)
         {
			
			var filteredActivationCodeRegistrations = _activationCodeRegistrationRepository.GetAll()
						.Include( e => e.ActivationCodeFk)
						.Include( e => e.RegistrationFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.ActivationCodeCodeFilter), e => e.ActivationCodeFk != null && e.ActivationCodeFk.Code == input.ActivationCodeCodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.RegistrationFirstNameFilter), e => e.RegistrationFk != null && e.RegistrationFk.FirstName == input.RegistrationFirstNameFilter);

			var pagedAndFilteredActivationCodeRegistrations = filteredActivationCodeRegistrations
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var activationCodeRegistrations = from o in pagedAndFilteredActivationCodeRegistrations
                         join o1 in _lookup_activationCodeRepository.GetAll() on o.ActivationCodeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_registrationRepository.GetAll() on o.RegistrationId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetActivationCodeRegistrationForViewDto() {
							ActivationCodeRegistration = new ActivationCodeRegistrationDto
							{
                                Id = o.Id
							},
                         	ActivationCodeCode = s1 == null || s1.Code == null ? "" : s1.Code.ToString(),
                         	RegistrationFirstName = s2 == null || s2.FirstName == null ? "" : s2.FirstName.ToString()
						};

            var totalCount = await filteredActivationCodeRegistrations.CountAsync();

            return new PagedResultDto<GetActivationCodeRegistrationForViewDto>(
                totalCount,
                await activationCodeRegistrations.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ActivationCodeRegistrations_Edit)]
		 public async Task<GetActivationCodeRegistrationForEditOutput> GetActivationCodeRegistrationForEdit(EntityDto<long> input)
         {
            var activationCodeRegistration = await _activationCodeRegistrationRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetActivationCodeRegistrationForEditOutput {ActivationCodeRegistration = ObjectMapper.Map<CreateOrEditActivationCodeRegistrationDto>(activationCodeRegistration)};

		    if (output.ActivationCodeRegistration.ActivationCodeId != null)
            {
                var _lookupActivationCode = await _lookup_activationCodeRepository.FirstOrDefaultAsync((long)output.ActivationCodeRegistration.ActivationCodeId);
                output.ActivationCodeCode = _lookupActivationCode?.Code?.ToString();
            }

		    if (output.ActivationCodeRegistration.RegistrationId != null)
            {
                var _lookupRegistration = await _lookup_registrationRepository.FirstOrDefaultAsync((long)output.ActivationCodeRegistration.RegistrationId);
                output.RegistrationFirstName = _lookupRegistration?.FirstName?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditActivationCodeRegistrationDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ActivationCodeRegistrations_Create)]
		 protected virtual async Task Create(CreateOrEditActivationCodeRegistrationDto input)
         {
            var activationCodeRegistration = ObjectMapper.Map<ActivationCodeRegistration>(input);

			
			if (AbpSession.TenantId != null)
			{
				activationCodeRegistration.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _activationCodeRegistrationRepository.InsertAsync(activationCodeRegistration);
         }

		 [AbpAuthorize(AppPermissions.Pages_ActivationCodeRegistrations_Edit)]
		 protected virtual async Task Update(CreateOrEditActivationCodeRegistrationDto input)
         {
            var activationCodeRegistration = await _activationCodeRegistrationRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, activationCodeRegistration);
         }

		 [AbpAuthorize(AppPermissions.Pages_ActivationCodeRegistrations_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _activationCodeRegistrationRepository.DeleteAsync(input.Id);
         } 

		[AbpAuthorize(AppPermissions.Pages_ActivationCodeRegistrations)]
         public async Task<PagedResultDto<ActivationCodeRegistrationActivationCodeLookupTableDto>> GetAllActivationCodeForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_activationCodeRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Code != null && e.Code.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var activationCodeList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ActivationCodeRegistrationActivationCodeLookupTableDto>();
			foreach(var activationCode in activationCodeList){
				lookupTableDtoList.Add(new ActivationCodeRegistrationActivationCodeLookupTableDto
				{
					Id = activationCode.Id,
					DisplayName = activationCode.Code?.ToString()
				});
			}

            return new PagedResultDto<ActivationCodeRegistrationActivationCodeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_ActivationCodeRegistrations)]
         public async Task<PagedResultDto<ActivationCodeRegistrationRegistrationLookupTableDto>> GetAllRegistrationForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_registrationRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.FirstName != null && e.FirstName.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var registrationList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ActivationCodeRegistrationRegistrationLookupTableDto>();
			foreach(var registration in registrationList){
				lookupTableDtoList.Add(new ActivationCodeRegistrationRegistrationLookupTableDto
				{
					Id = registration.Id,
					DisplayName = registration.FirstName?.ToString()
				});
			}

            return new PagedResultDto<ActivationCodeRegistrationRegistrationLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}