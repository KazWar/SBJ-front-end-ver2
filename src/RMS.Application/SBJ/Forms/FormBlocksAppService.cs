using RMS.SBJ.Forms;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.Forms.Exporting;
using RMS.SBJ.Forms.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.Forms
{
	[AbpAuthorize(AppPermissions.Pages_FormBlocks)]
    public class FormBlocksAppService : RMSAppServiceBase, IFormBlocksAppService
    {
		 private readonly IRepository<FormBlock, long> _formBlockRepository;
		 private readonly IFormBlocksExcelExporter _formBlocksExcelExporter;
		 private readonly IRepository<FormLocale,long> _lookup_formLocaleRepository;
		 

		  public FormBlocksAppService(IRepository<FormBlock, long> formBlockRepository, IFormBlocksExcelExporter formBlocksExcelExporter , IRepository<FormLocale, long> lookup_formLocaleRepository) 
		  {
			_formBlockRepository = formBlockRepository;
			_formBlocksExcelExporter = formBlocksExcelExporter;
			_lookup_formLocaleRepository = lookup_formLocaleRepository;
		
		  }

		 public async Task<PagedResultDto<GetFormBlockForViewDto>> GetAll(GetAllFormBlocksInput input)
         {
			
			var filteredFormBlocks = _formBlockRepository.GetAll()
						.Include( e => e.FormLocaleFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(input.IsPurchaseRegistrationFilter.HasValue && input.IsPurchaseRegistrationFilter > -1,  e => (input.IsPurchaseRegistrationFilter == 1 && e.IsPurchaseRegistration) || (input.IsPurchaseRegistrationFilter == 0 && !e.IsPurchaseRegistration) )
						.WhereIf(input.MinSortOrderFilter != null, e => e.SortOrder >= input.MinSortOrderFilter)
						.WhereIf(input.MaxSortOrderFilter != null, e => e.SortOrder <= input.MaxSortOrderFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.FormLocaleDescriptionFilter), e => e.FormLocaleFk != null && e.FormLocaleFk.Description == input.FormLocaleDescriptionFilter);

			var pagedAndFilteredFormBlocks = filteredFormBlocks
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var formBlocks = from o in pagedAndFilteredFormBlocks
                         join o1 in _lookup_formLocaleRepository.GetAll() on o.FormLocaleId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetFormBlockForViewDto() {
							FormBlock = new FormBlockDto
							{
                                Description = o.Description,
                                IsPurchaseRegistration = o.IsPurchaseRegistration,
                                SortOrder = o.SortOrder,
                                Id = o.Id
							},
                         	FormLocaleDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString()
						};

            var totalCount = await filteredFormBlocks.CountAsync();

            return new PagedResultDto<GetFormBlockForViewDto>(
                totalCount,
                await formBlocks.ToListAsync()
            );
         }
		 
		 public async Task<GetFormBlockForViewDto> GetFormBlockForView(long id)
         {
            var formBlock = await _formBlockRepository.GetAsync(id);

            var output = new GetFormBlockForViewDto { FormBlock = ObjectMapper.Map<FormBlockDto>(formBlock) };

		    if (output.FormBlock.FormLocaleId != null)
            {
                var _lookupFormLocale = await _lookup_formLocaleRepository.FirstOrDefaultAsync((long)output.FormBlock.FormLocaleId);
                output.FormLocaleDescription = _lookupFormLocale?.Description?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_FormBlocks_Edit)]
		 public async Task<GetFormBlockForEditOutput> GetFormBlockForEdit(EntityDto<long> input)
         {
            var formBlock = await _formBlockRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetFormBlockForEditOutput {FormBlock = ObjectMapper.Map<CreateOrEditFormBlockDto>(formBlock)};

		    if (output.FormBlock.FormLocaleId != null)
            {
                var _lookupFormLocale = await _lookup_formLocaleRepository.FirstOrDefaultAsync((long)output.FormBlock.FormLocaleId);
                output.FormLocaleDescription = _lookupFormLocale?.Description?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditFormBlockDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

        public async Task<long> CreateOrEditAndGetId(CreateOrEditFormBlockDto input)
        {
            var formBlock = ObjectMapper.Map<FormBlock>(input);
            if (AbpSession.TenantId != null)
            {
                formBlock.TenantId = (int?)AbpSession.TenantId;
            }
            if (input.Id == null)
            {
                var formBlockId = await _formBlockRepository.InsertAndGetIdAsync(formBlock);
                return formBlockId;
            }
            else
            {
                var formBlockId = await _formBlockRepository.InsertOrUpdateAndGetIdAsync(formBlock);
                return formBlockId;
            }
        }


         [AbpAuthorize(AppPermissions.Pages_FormBlocks_Create)]
		 protected virtual async Task Create(CreateOrEditFormBlockDto input)
         {
            var formBlock = ObjectMapper.Map<FormBlock>(input);

			
			if (AbpSession.TenantId != null)
			{
				formBlock.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _formBlockRepository.InsertAsync(formBlock);
         }

		 [AbpAuthorize(AppPermissions.Pages_FormBlocks_Edit)]
		 protected virtual async Task Update(CreateOrEditFormBlockDto input)
         {
            var formBlock = await _formBlockRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, formBlock);
         }

		 [AbpAuthorize(AppPermissions.Pages_FormBlocks_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _formBlockRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetFormBlocksToExcel(GetAllFormBlocksForExcelInput input)
         {
			
			var filteredFormBlocks = _formBlockRepository.GetAll()
						.Include( e => e.FormLocaleFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(input.IsPurchaseRegistrationFilter.HasValue && input.IsPurchaseRegistrationFilter > -1,  e => (input.IsPurchaseRegistrationFilter == 1 && e.IsPurchaseRegistration) || (input.IsPurchaseRegistrationFilter == 0 && !e.IsPurchaseRegistration) )
						.WhereIf(input.MinSortOrderFilter != null, e => e.SortOrder >= input.MinSortOrderFilter)
						.WhereIf(input.MaxSortOrderFilter != null, e => e.SortOrder <= input.MaxSortOrderFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.FormLocaleDescriptionFilter), e => e.FormLocaleFk != null && e.FormLocaleFk.Description == input.FormLocaleDescriptionFilter);

			var query = (from o in filteredFormBlocks
                         join o1 in _lookup_formLocaleRepository.GetAll() on o.FormLocaleId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetFormBlockForViewDto() { 
							FormBlock = new FormBlockDto
							{
                                Description = o.Description,
                                IsPurchaseRegistration = o.IsPurchaseRegistration,
                                SortOrder = o.SortOrder,
                                Id = o.Id
							},
                         	FormLocaleDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString()
						 });


            var formBlockListDtos = await query.ToListAsync();

            return _formBlocksExcelExporter.ExportToFile(formBlockListDtos);
        }

		[AbpAuthorize(AppPermissions.Pages_FormBlocks)]
         public async Task<PagedResultDto<FormBlockFormLocaleLookupTableDto>> GetAllFormLocaleForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_formLocaleRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Description != null && e.Description.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var formLocaleList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<FormBlockFormLocaleLookupTableDto>();
			foreach(var formLocale in formLocaleList){
				lookupTableDtoList.Add(new FormBlockFormLocaleLookupTableDto
				{
					Id = formLocale.Id,
					DisplayName = formLocale.Description?.ToString()
				});
			}

            return new PagedResultDto<FormBlockFormLocaleLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public async Task<List<GetFormBlockForViewDto>> GetAllFormBlocks()
        {
            var allFormBlocks = _formBlockRepository.GetAll()
                        .Include(e => e.FormLocaleFk);

            var formBlocks = from o in allFormBlocks
                             join o1 in _lookup_formLocaleRepository.GetAll() on o.FormLocaleId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             select new GetFormBlockForViewDto()
                             {
                                 FormBlock = new FormBlockDto
                                 {                                    
                                     Id = o.Id,
                                     FormLocaleId = o.FormLocaleId,
                                     Description = o.Description,
                                     IsPurchaseRegistration = o.IsPurchaseRegistration,
                                     SortOrder = o.SortOrder
                                 },
                                 FormLocaleDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString()
                             };

            var totalCount = await allFormBlocks.CountAsync();

            return await formBlocks.ToListAsync();
        }
    }
}