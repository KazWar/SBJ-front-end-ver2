using RMS.SBJ.Forms;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.CampaignProcesses.Exporting;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using RMS.SBJ.CodeTypeTables;


namespace RMS.SBJ.CampaignProcesses
{
    [AbpAuthorize(AppPermissions.Pages_CampaignForms)]
    public class CampaignFormsAppService : RMSAppServiceBase, ICampaignFormsAppService
    {
		private readonly IRepository<CampaignForm, long> _campaignFormRepository;
		private readonly ICampaignFormsExcelExporter _campaignFormsExcelExporter;
		private readonly IRepository<Campaign,long> _lookup_campaignRepository;
		private readonly IRepository<Form,long> _lookup_formRepository;
        private readonly IRepository<Company.Company, long> _companyRepository;
        private readonly IRepository<FormLocale, long> _formLocaleRepository;
        private readonly IRepository<Locale, long> _lookup_localeRepository;



        public CampaignFormsAppService(
            IRepository<CampaignForm, long> campaignFormRepository,
            ICampaignFormsExcelExporter campaignFormsExcelExporter,
            IRepository<Campaign, long> lookup_campaignRepository,
            IRepository<Form, long> lookup_formRepository,
            IRepository<Company.Company, long> companyRepository,
            IRepository<FormLocale, long> formLocaleRepository,
            IRepository<Locale, long> lookup_localeRepository
            ) 
		  {
			_campaignFormRepository = campaignFormRepository;
			_campaignFormsExcelExporter = campaignFormsExcelExporter;
		    _lookup_campaignRepository = lookup_campaignRepository;
		    _lookup_formRepository = lookup_formRepository;
            _companyRepository = companyRepository;
            _formLocaleRepository = formLocaleRepository;
            _lookup_localeRepository = lookup_localeRepository;
        }

        

        public async Task<PagedResultDto<GetCampaignFormForViewDto>> GetAll(GetAllCampaignFormsInput input)
         {
			
			var filteredCampaignForms = _campaignFormRepository.GetAll()
						.Include( e => e.CampaignFk)
						.Include( e => e.FormFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.CampaignNameFilter), e => e.CampaignFk != null && e.CampaignFk.Name == input.CampaignNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.FormVersionFilter), e => e.FormFk != null && e.FormFk.Version == input.FormVersionFilter);

			var pagedAndFilteredCampaignForms = filteredCampaignForms
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var campaignForms = from o in pagedAndFilteredCampaignForms
                         join o1 in _lookup_campaignRepository.GetAll() on o.CampaignId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_formRepository.GetAll() on o.FormId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetCampaignFormForViewDto() {
							CampaignForm = new CampaignFormDto
							{
                                IsActive = o.IsActive,
                                Id = o.Id,
                                CampaignId = o.CampaignId,
                                FormId = o.FormId
							},
                         	CampaignName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                         	FormVersion = s2 == null || s2.Version == null ? "" : s2.Version.ToString()
						};

            var totalCount = await filteredCampaignForms.CountAsync();

            return new PagedResultDto<GetCampaignFormForViewDto>(
                totalCount,
                await campaignForms.ToListAsync()
            );
         }
		 
		 public async Task<GetCampaignFormForViewDto> GetCampaignFormForView(long id)
         {
            var campaignForm = await _campaignFormRepository.GetAsync(id);

            var output = new GetCampaignFormForViewDto { CampaignForm = ObjectMapper.Map<CampaignFormDto>(campaignForm) };

		    if (output.CampaignForm.CampaignId != null)
            {
                var _lookupCampaign = await _lookup_campaignRepository.FirstOrDefaultAsync((long)output.CampaignForm.CampaignId);
                output.CampaignName = _lookupCampaign?.Name?.ToString();
            }

		    if (output.CampaignForm.FormId != null)
            {
                var _lookupForm = await _lookup_formRepository.FirstOrDefaultAsync((long)output.CampaignForm.FormId);
                output.FormVersion = _lookupForm?.Version?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_CampaignForms_Edit)]
		 public async Task<GetCampaignFormForEditOutput> GetCampaignFormForEdit(EntityDto<long> input)
         {
            var campaignForm = await _campaignFormRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetCampaignFormForEditOutput {CampaignForm = ObjectMapper.Map<CreateOrEditCampaignFormDto>(campaignForm)};

		    if (output.CampaignForm.CampaignId != null)
            {
                var _lookupCampaign = await _lookup_campaignRepository.FirstOrDefaultAsync((long)output.CampaignForm.CampaignId);
                output.CampaignName = _lookupCampaign?.Name?.ToString();
            }

		    if (output.CampaignForm.FormId != null)
            {
                var _lookupForm = await _lookup_formRepository.FirstOrDefaultAsync((long)output.CampaignForm.FormId);
                output.FormVersion = _lookupForm?.Version?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditCampaignFormDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_CampaignForms_Create)]
		 protected virtual async Task Create(CreateOrEditCampaignFormDto input)
         {
            var campaignForm = ObjectMapper.Map<CampaignForm>(input);

			
			if (AbpSession.TenantId != null)
			{
				campaignForm.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _campaignFormRepository.InsertAsync(campaignForm);
         }

		 [AbpAuthorize(AppPermissions.Pages_CampaignForms_Edit)]
		 protected virtual async Task Update(CreateOrEditCampaignFormDto input)
         {
            var campaignForm = await _campaignFormRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, campaignForm);
         }

		 [AbpAuthorize(AppPermissions.Pages_CampaignForms_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _campaignFormRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetCampaignFormsToExcel(GetAllCampaignFormsForExcelInput input)
         {
			
			var filteredCampaignForms = _campaignFormRepository.GetAll()
						.Include( e => e.CampaignFk)
						.Include( e => e.FormFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.CampaignNameFilter), e => e.CampaignFk != null && e.CampaignFk.Name == input.CampaignNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.FormVersionFilter), e => e.FormFk != null && e.FormFk.Version == input.FormVersionFilter);

			var query = (from o in filteredCampaignForms
                         join o1 in _lookup_campaignRepository.GetAll() on o.CampaignId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_formRepository.GetAll() on o.FormId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetCampaignFormForViewDto() { 
							CampaignForm = new CampaignFormDto
							{
                                IsActive = o.IsActive,
                                Id = o.Id
							},
                         	CampaignName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                         	FormVersion = s2 == null || s2.Version == null ? "" : s2.Version.ToString()
						 });


            var campaignFormListDtos = await query.ToListAsync();

            return _campaignFormsExcelExporter.ExportToFile(campaignFormListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_CampaignForms)]
         public async Task<PagedResultDto<CampaignFormCampaignLookupTableDto>> GetAllCampaignForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_campaignRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var campaignList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<CampaignFormCampaignLookupTableDto>();
			foreach(var campaign in campaignList){
				lookupTableDtoList.Add(new CampaignFormCampaignLookupTableDto
				{
					Id = campaign.Id,
					DisplayName = campaign.Name?.ToString()
				});
			}

            return new PagedResultDto<CampaignFormCampaignLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_CampaignForms)]
         public async Task<PagedResultDto<CampaignFormFormLookupTableDto>> GetAllFormForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_formRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Version != null && e.Version.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var formList = await query
                .PageBy(input)
                .ToListAsync();

            var campaignFormList = formList.Where(item => item.SystemLevelId == input.FormSelectionPageSource);

			var lookupTableDtoList = new List<CampaignFormFormLookupTableDto>();
			foreach(var form in campaignFormList)
            {
				lookupTableDtoList.Add(new CampaignFormFormLookupTableDto
				{
					Id = form.Id,
					DisplayName = form.Version?.ToString()
				});
			}

            return new PagedResultDto<CampaignFormFormLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

        public async Task<List<GetCampaignFormForViewDto>> GetAllCampaignForms()
        {

            var allCampaignForms = _campaignFormRepository.GetAll()
                        .Include(e => e.CampaignFk)
                        .Include(e => e.FormFk);

            var campaignForms = from o in allCampaignForms
                                join o1 in _lookup_campaignRepository.GetAll() on o.CampaignId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()

                                join o2 in _lookup_formRepository.GetAll() on o.FormId equals o2.Id into j2
                                from s2 in j2.DefaultIfEmpty()

                                select new GetCampaignFormForViewDto()
                                {
                                    CampaignForm = new CampaignFormDto
                                    {
                                        IsActive = o.IsActive,
                                        Id = o.Id,
                                        CampaignId = o.CampaignId,
                                        FormId = o.FormId
                                    },
                                    CampaignName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                    FormVersion = s2 == null || s2.Version == null ? "" : s2.Version.ToString()
                                };

            var totalCount = await allCampaignForms.CountAsync();

            return await campaignForms.ToListAsync();
        }


    }
}