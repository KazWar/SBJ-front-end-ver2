using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using RMS.Authorization;
using RMS.Dto;
using RMS.SBJ.Forms.Dtos;
using RMS.SBJ.Forms.Exporting;
using RMS.SBJ.SystemTables;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace RMS.SBJ.Forms
{
    [AbpAuthorize(AppPermissions.Pages_Forms)]
    public class FormsAppService : RMSAppServiceBase, IFormsAppService
    {
		 private readonly IRepository<Form, long> _formRepository;
		 private readonly IFormsExcelExporter _formsExcelExporter;
		 private readonly IRepository<SystemLevel,long> _lookup_systemLevelRepository;
		 

		  public FormsAppService(IRepository<Form, long> formRepository, IFormsExcelExporter formsExcelExporter , IRepository<SystemLevel, long> lookup_systemLevelRepository) 
		  {
			_formRepository = formRepository;
			_formsExcelExporter = formsExcelExporter;
			_lookup_systemLevelRepository = lookup_systemLevelRepository;
		
		  }

		 public async Task<PagedResultDto<GetFormForViewDto>> GetAll(GetAllFormsInput input)
         {

            
			
			var filteredForms = _formRepository.GetAll()
						.Include( e => e.SystemLevelFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Version.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.VersionFilter),  e => e.Version == input.VersionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SystemLevelDescriptionFilter), e => e.SystemLevelFk != null && e.SystemLevelFk.Description == input.SystemLevelDescriptionFilter);

			var pagedAndFilteredForms = filteredForms
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var forms = from o in pagedAndFilteredForms
                         join o1 in _lookup_systemLevelRepository.GetAll() on o.SystemLevelId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetFormForViewDto() {
							Form = new FormDto
							{
                                Version = o.Version,
                                Id = o.Id,
                                SystemLevelId = o.SystemLevelId,
							},
                         	SystemLevelDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString()
						};

            var totalCount = await filteredForms.CountAsync();

            return new PagedResultDto<GetFormForViewDto>(
                totalCount,
                await forms.ToListAsync()
            );
         }
		 
		 public async Task<GetFormForViewDto> GetFormForView(long id)
         {
            var form = await _formRepository.GetAsync(id);

            var output = new GetFormForViewDto { Form = ObjectMapper.Map<FormDto>(form) };

		    if (output.Form.SystemLevelId != null)
            {
                var _lookupSystemLevel = await _lookup_systemLevelRepository.FirstOrDefaultAsync((long)output.Form.SystemLevelId);
                output.SystemLevelDescription = _lookupSystemLevel?.Description?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Forms_Edit)]
		 public async Task<GetFormForEditOutput> GetFormForEdit(EntityDto<long> input)
         {
            var form = await _formRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetFormForEditOutput {Form = ObjectMapper.Map<CreateOrEditFormDto>(form)};

		    if (output.Form.SystemLevelId != null)
            {
                var _lookupSystemLevel = await _lookup_systemLevelRepository.FirstOrDefaultAsync((long)output.Form.SystemLevelId);
                output.SystemLevelDescription = _lookupSystemLevel?.Description?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditFormDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Forms_Create)]
		 protected virtual async Task Create(CreateOrEditFormDto input)
         {
            var form = ObjectMapper.Map<Form>(input);

			
			if (AbpSession.TenantId != null)
			{
				form.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _formRepository.InsertAsync(form);
         }

		 [AbpAuthorize(AppPermissions.Pages_Forms_Edit)]
		 protected virtual async Task Update(CreateOrEditFormDto input)
         {
            var form = await _formRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, form);
         }

		 [AbpAuthorize(AppPermissions.Pages_Forms_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _formRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetFormsToExcel(GetAllFormsForExcelInput input)
         {
			
			var filteredForms = _formRepository.GetAll()
						.Include( e => e.SystemLevelFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Version.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.VersionFilter),  e => e.Version == input.VersionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SystemLevelDescriptionFilter), e => e.SystemLevelFk != null && e.SystemLevelFk.Description == input.SystemLevelDescriptionFilter);

			var query = (from o in filteredForms
                         join o1 in _lookup_systemLevelRepository.GetAll() on o.SystemLevelId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetFormForViewDto() { 
							Form = new FormDto
							{
                                Version = o.Version,
                                Id = o.Id
							},
                         	SystemLevelDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString()
						 });


            var formListDtos = await query.ToListAsync();

            return _formsExcelExporter.ExportToFile(formListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Forms)]
         public async Task<PagedResultDto<FormSystemLevelLookupTableDto>> GetAllSystemLevelForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_systemLevelRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Description != null && e.Description.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var systemLevelList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<FormSystemLevelLookupTableDto>();
			foreach(var systemLevel in systemLevelList){
				lookupTableDtoList.Add(new FormSystemLevelLookupTableDto
				{
					Id = systemLevel.Id,
					DisplayName = systemLevel.Description?.ToString()
				});
			}

            return new PagedResultDto<FormSystemLevelLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

        public async Task<List<GetFormForViewDto>> GetAllForms()
        {
            var allForms = _formRepository.GetAll()
                        .Include(e => e.SystemLevelFk);

            var forms = from o in allForms
                        join o1 in _lookup_systemLevelRepository.GetAll() on o.SystemLevelId equals o1.Id into j1
                        from s1 in j1.DefaultIfEmpty()

                        select new GetFormForViewDto()
                        {
                            Form = new FormDto
                            {
                                Version = o.Version,
                                Id = o.Id,
                                SystemLevelId = o.SystemLevelId,
                            },
                            SystemLevelDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString()
                        };

            var totalCount = await allForms.CountAsync();

            return forms.ToList();
            
        }
    }
}