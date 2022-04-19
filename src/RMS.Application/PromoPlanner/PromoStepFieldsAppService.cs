using RMS.PromoPlanner;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.PromoPlanner.Exporting;
using RMS.PromoPlanner.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.PromoPlanner
{
	[AbpAuthorize(AppPermissions.Pages_PromoStepFields)]
    public class PromoStepFieldsAppService : RMSAppServiceBase, IPromoStepFieldsAppService
    {
		 private readonly IRepository<PromoStepField> _promoStepFieldRepository;
		 private readonly IPromoStepFieldsExcelExporter _promoStepFieldsExcelExporter;
		 private readonly IRepository<PromoStep,int> _lookup_promoStepRepository;
		 

		  public PromoStepFieldsAppService(IRepository<PromoStepField> promoStepFieldRepository, IPromoStepFieldsExcelExporter promoStepFieldsExcelExporter , IRepository<PromoStep, int> lookup_promoStepRepository) 
		  {
			_promoStepFieldRepository = promoStepFieldRepository;
			_promoStepFieldsExcelExporter = promoStepFieldsExcelExporter;
			_lookup_promoStepRepository = lookup_promoStepRepository;
		
		  }

		 public async Task<PagedResultDto<GetPromoStepFieldForViewDto>> GetAll(GetAllPromoStepFieldsInput input)
         {
			
			var filteredPromoStepFields = _promoStepFieldRepository.GetAll()
						.Include( e => e.PromoStepFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter))
						.WhereIf(input.MinFormFieldIdFilter != null, e => e.FormFieldId >= input.MinFormFieldIdFilter)
						.WhereIf(input.MaxFormFieldIdFilter != null, e => e.FormFieldId <= input.MaxFormFieldIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(input.MinSequenceFilter != null, e => e.Sequence >= input.MinSequenceFilter)
						.WhereIf(input.MaxSequenceFilter != null, e => e.Sequence <= input.MaxSequenceFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PromoStepDescriptionFilter), e => e.PromoStepFk != null && e.PromoStepFk.Description == input.PromoStepDescriptionFilter);

			var pagedAndFilteredPromoStepFields = filteredPromoStepFields
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var promoStepFields = from o in pagedAndFilteredPromoStepFields
                         join o1 in _lookup_promoStepRepository.GetAll() on o.PromoStepId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetPromoStepFieldForViewDto() {
							PromoStepField = new PromoStepFieldDto
							{
                                FormFieldId = o.FormFieldId,
                                Description = o.Description,
                                Sequence = o.Sequence,
                                Id = o.Id
							},
                         	PromoStepDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString()
						};

            var totalCount = await filteredPromoStepFields.CountAsync();

            return new PagedResultDto<GetPromoStepFieldForViewDto>(
                totalCount,
                await promoStepFields.ToListAsync()
            );
         }
		 
		 public async Task<GetPromoStepFieldForViewDto> GetPromoStepFieldForView(int id)
         {
            var promoStepField = await _promoStepFieldRepository.GetAsync(id);

            var output = new GetPromoStepFieldForViewDto { PromoStepField = ObjectMapper.Map<PromoStepFieldDto>(promoStepField) };

		    if (output.PromoStepField.PromoStepId != null)
            {
                var _lookupPromoStep = await _lookup_promoStepRepository.FirstOrDefaultAsync((int)output.PromoStepField.PromoStepId);
                output.PromoStepDescription = _lookupPromoStep?.Description?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_PromoStepFields_Edit)]
		 public async Task<GetPromoStepFieldForEditOutput> GetPromoStepFieldForEdit(EntityDto input)
         {
            var promoStepField = await _promoStepFieldRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetPromoStepFieldForEditOutput {PromoStepField = ObjectMapper.Map<CreateOrEditPromoStepFieldDto>(promoStepField)};

		    if (output.PromoStepField.PromoStepId != null)
            {
                var _lookupPromoStep = await _lookup_promoStepRepository.FirstOrDefaultAsync((int)output.PromoStepField.PromoStepId);
                output.PromoStepDescription = _lookupPromoStep?.Description?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditPromoStepFieldDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_PromoStepFields_Create)]
		 protected virtual async Task Create(CreateOrEditPromoStepFieldDto input)
         {
            var promoStepField = ObjectMapper.Map<PromoStepField>(input);

			
			if (AbpSession.TenantId != null)
			{
				promoStepField.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _promoStepFieldRepository.InsertAsync(promoStepField);
         }

		 [AbpAuthorize(AppPermissions.Pages_PromoStepFields_Edit)]
		 protected virtual async Task Update(CreateOrEditPromoStepFieldDto input)
         {
            var promoStepField = await _promoStepFieldRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, promoStepField);
         }

		 [AbpAuthorize(AppPermissions.Pages_PromoStepFields_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _promoStepFieldRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetPromoStepFieldsToExcel(GetAllPromoStepFieldsForExcelInput input)
         {
			
			var filteredPromoStepFields = _promoStepFieldRepository.GetAll()
						.Include( e => e.PromoStepFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter))
						.WhereIf(input.MinFormFieldIdFilter != null, e => e.FormFieldId >= input.MinFormFieldIdFilter)
						.WhereIf(input.MaxFormFieldIdFilter != null, e => e.FormFieldId <= input.MaxFormFieldIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(input.MinSequenceFilter != null, e => e.Sequence >= input.MinSequenceFilter)
						.WhereIf(input.MaxSequenceFilter != null, e => e.Sequence <= input.MaxSequenceFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PromoStepDescriptionFilter), e => e.PromoStepFk != null && e.PromoStepFk.Description == input.PromoStepDescriptionFilter);

			var query = (from o in filteredPromoStepFields
                         join o1 in _lookup_promoStepRepository.GetAll() on o.PromoStepId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetPromoStepFieldForViewDto() { 
							PromoStepField = new PromoStepFieldDto
							{
                                FormFieldId = o.FormFieldId,
                                Description = o.Description,
                                Sequence = o.Sequence,
                                Id = o.Id
							},
                         	PromoStepDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString()
						 });


            var promoStepFieldListDtos = await query.ToListAsync();

            return _promoStepFieldsExcelExporter.ExportToFile(promoStepFieldListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_PromoStepFields)]
         public async Task<PagedResultDto<PromoStepFieldPromoStepLookupTableDto>> GetAllPromoStepForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_promoStepRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Description != null && e.Description.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var promoStepList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<PromoStepFieldPromoStepLookupTableDto>();
			foreach(var promoStep in promoStepList){
				lookupTableDtoList.Add(new PromoStepFieldPromoStepLookupTableDto
				{
					Id = promoStep.Id,
					DisplayName = promoStep.Description?.ToString()
				});
			}

            return new PagedResultDto<PromoStepFieldPromoStepLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}