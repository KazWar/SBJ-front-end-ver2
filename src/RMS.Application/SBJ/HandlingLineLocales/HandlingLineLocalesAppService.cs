using RMS.SBJ.HandlingLines;
using RMS.SBJ.CodeTypeTables;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.HandlingLineLocales.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.HandlingLineLocales
{
	[AbpAuthorize(AppPermissions.Pages_HandlingLineLocales)]
    public class HandlingLineLocalesAppService : RMSAppServiceBase, IHandlingLineLocalesAppService
    {
		 private readonly IRepository<HandlingLineLocale, long> _handlingLineLocaleRepository;
		 private readonly IRepository<HandlingLine,long> _lookup_handlingLineRepository;
		 private readonly IRepository<Locale,long> _lookup_localeRepository;
		 

		  public HandlingLineLocalesAppService(IRepository<HandlingLineLocale, long> handlingLineLocaleRepository , IRepository<HandlingLine, long> lookup_handlingLineRepository, IRepository<Locale, long> lookup_localeRepository) 
		  {
			_handlingLineLocaleRepository = handlingLineLocaleRepository;
			_lookup_handlingLineRepository = lookup_handlingLineRepository;
		_lookup_localeRepository = lookup_localeRepository;
		
		  }

		 public async Task<PagedResultDto<GetHandlingLineLocaleForViewDto>> GetAll(GetAllHandlingLineLocalesInput input)
         {
			
			var filteredHandlingLineLocales = _handlingLineLocaleRepository.GetAll()
						.Include( e => e.HandlingLineFk)
						.Include( e => e.LocaleFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.HandlingLineCustomerCodeFilter), e => e.HandlingLineFk != null && e.HandlingLineFk.CustomerCode == input.HandlingLineCustomerCodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LocaleLanguageCodeFilter), e => e.LocaleFk != null && e.LocaleFk.LanguageCode == input.LocaleLanguageCodeFilter);

			var pagedAndFilteredHandlingLineLocales = filteredHandlingLineLocales
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var handlingLineLocales = from o in pagedAndFilteredHandlingLineLocales
                         join o1 in _lookup_handlingLineRepository.GetAll() on o.HandlingLineId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_localeRepository.GetAll() on o.LocaleId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetHandlingLineLocaleForViewDto() {
							HandlingLineLocale = new HandlingLineLocaleDto
							{
                                Id = o.Id
							},
                         	HandlingLineCustomerCode = s1 == null || s1.CustomerCode == null ? "" : s1.CustomerCode.ToString(),
                         	LocaleLanguageCode = s2 == null || s2.LanguageCode == null ? "" : s2.LanguageCode.ToString()
						};

            var totalCount = await filteredHandlingLineLocales.CountAsync();

            return new PagedResultDto<GetHandlingLineLocaleForViewDto>(
                totalCount,
                await handlingLineLocales.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_HandlingLineLocales_Edit)]
		 public async Task<GetHandlingLineLocaleForEditOutput> GetHandlingLineLocaleForEdit(EntityDto<long> input)
         {
            var handlingLineLocale = await _handlingLineLocaleRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetHandlingLineLocaleForEditOutput {HandlingLineLocale = ObjectMapper.Map<CreateOrEditHandlingLineLocaleDto>(handlingLineLocale)};

		    if (output.HandlingLineLocale.HandlingLineId != null)
            {
                var _lookupHandlingLine = await _lookup_handlingLineRepository.FirstOrDefaultAsync((long)output.HandlingLineLocale.HandlingLineId);
                output.HandlingLineCustomerCode = _lookupHandlingLine?.CustomerCode?.ToString();
            }

		    if (output.HandlingLineLocale.LocaleId != null)
            {
                var _lookupLocale = await _lookup_localeRepository.FirstOrDefaultAsync((long)output.HandlingLineLocale.LocaleId);
                output.LocaleLanguageCode = _lookupLocale?.LanguageCode?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditHandlingLineLocaleDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_HandlingLineLocales_Create)]
		 protected virtual async Task Create(CreateOrEditHandlingLineLocaleDto input)
         {
            var handlingLineLocale = ObjectMapper.Map<HandlingLineLocale>(input);

			
			if (AbpSession.TenantId != null)
			{
				handlingLineLocale.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _handlingLineLocaleRepository.InsertAsync(handlingLineLocale);
         }

		 [AbpAuthorize(AppPermissions.Pages_HandlingLineLocales_Edit)]
		 protected virtual async Task Update(CreateOrEditHandlingLineLocaleDto input)
         {
            var handlingLineLocale = await _handlingLineLocaleRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, handlingLineLocale);
         }

		 [AbpAuthorize(AppPermissions.Pages_HandlingLineLocales_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _handlingLineLocaleRepository.DeleteAsync(input.Id);
         } 

		[AbpAuthorize(AppPermissions.Pages_HandlingLineLocales)]
         public async Task<PagedResultDto<HandlingLineLocaleHandlingLineLookupTableDto>> GetAllHandlingLineForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_handlingLineRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.CustomerCode != null && e.CustomerCode.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var handlingLineList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<HandlingLineLocaleHandlingLineLookupTableDto>();
			foreach(var handlingLine in handlingLineList){
				lookupTableDtoList.Add(new HandlingLineLocaleHandlingLineLookupTableDto
				{
					Id = handlingLine.Id,
					DisplayName = handlingLine.CustomerCode?.ToString()
				});
			}

            return new PagedResultDto<HandlingLineLocaleHandlingLineLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_HandlingLineLocales)]
         public async Task<PagedResultDto<HandlingLineLocaleLocaleLookupTableDto>> GetAllLocaleForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_localeRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.LanguageCode != null && e.LanguageCode.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var localeList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<HandlingLineLocaleLocaleLookupTableDto>();
			foreach(var locale in localeList){
				lookupTableDtoList.Add(new HandlingLineLocaleLocaleLookupTableDto
				{
					Id = locale.Id,
					DisplayName = locale.LanguageCode?.ToString()
				});
			}

            return new PagedResultDto<HandlingLineLocaleLocaleLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}