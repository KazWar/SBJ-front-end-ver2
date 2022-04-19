using RMS.SBJ.CodeTypeTables;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.ActivationCodes.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.ActivationCodes
{
	[AbpAuthorize(AppPermissions.Pages_ActivationCodes)]
    public class ActivationCodesAppService : RMSAppServiceBase, IActivationCodesAppService
    {
		 private readonly IRepository<ActivationCode, long> _activationCodeRepository;
		 private readonly IRepository<Locale,long> _lookup_localeRepository;
		 

		  public ActivationCodesAppService(IRepository<ActivationCode, long> activationCodeRepository , IRepository<Locale, long> lookup_localeRepository) 
		  {
			_activationCodeRepository = activationCodeRepository;
			_lookup_localeRepository = lookup_localeRepository;
		
		  }

		 public async Task<PagedResultDto<GetActivationCodeForViewDto>> GetAll(GetAllActivationCodesInput input)
         {
			
			var filteredActivationCodes = _activationCodeRepository.GetAll()
						.Include( e => e.LocaleFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Code.Contains(input.Filter) || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LocaleLanguageCodeFilter), e => e.LocaleFk != null && e.LocaleFk.LanguageCode == input.LocaleLanguageCodeFilter);

			var pagedAndFilteredActivationCodes = filteredActivationCodes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var activationCodes = from o in pagedAndFilteredActivationCodes
                         join o1 in _lookup_localeRepository.GetAll() on o.LocaleId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetActivationCodeForViewDto() {
							ActivationCode = new ActivationCodeDto
							{
                                Code = o.Code,
                                Description = o.Description,
                                Id = o.Id
							},
                         	LocaleLanguageCode = s1 == null || s1.LanguageCode == null ? "" : s1.LanguageCode.ToString()
						};

            var totalCount = await filteredActivationCodes.CountAsync();

            return new PagedResultDto<GetActivationCodeForViewDto>(
                totalCount,
                await activationCodes.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ActivationCodes_Edit)]
		 public async Task<GetActivationCodeForEditOutput> GetActivationCodeForEdit(EntityDto<long> input)
         {
            var activationCode = await _activationCodeRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetActivationCodeForEditOutput {ActivationCode = ObjectMapper.Map<CreateOrEditActivationCodeDto>(activationCode)};

		    if (output.ActivationCode.LocaleId != null)
            {
                var _lookupLocale = await _lookup_localeRepository.FirstOrDefaultAsync((long)output.ActivationCode.LocaleId);
                output.LocaleLanguageCode = _lookupLocale?.LanguageCode?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditActivationCodeDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ActivationCodes_Create)]
		 protected virtual async Task Create(CreateOrEditActivationCodeDto input)
         {
            var activationCode = ObjectMapper.Map<ActivationCode>(input);

			
			if (AbpSession.TenantId != null)
			{
				activationCode.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _activationCodeRepository.InsertAsync(activationCode);
         }

		 [AbpAuthorize(AppPermissions.Pages_ActivationCodes_Edit)]
		 protected virtual async Task Update(CreateOrEditActivationCodeDto input)
         {
            var activationCode = await _activationCodeRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, activationCode);
         }

		 [AbpAuthorize(AppPermissions.Pages_ActivationCodes_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _activationCodeRepository.DeleteAsync(input.Id);
         } 

		[AbpAuthorize(AppPermissions.Pages_ActivationCodes)]
         public async Task<PagedResultDto<ActivationCodeLocaleLookupTableDto>> GetAllLocaleForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_localeRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.LanguageCode != null && e.LanguageCode.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var localeList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ActivationCodeLocaleLookupTableDto>();
			foreach(var locale in localeList){
				lookupTableDtoList.Add(new ActivationCodeLocaleLookupTableDto
				{
					Id = locale.Id,
					DisplayName = locale.LanguageCode?.ToString()
				});
			}

            return new PagedResultDto<ActivationCodeLocaleLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}