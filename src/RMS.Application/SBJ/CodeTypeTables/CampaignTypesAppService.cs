using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.CodeTypeTables.Exporting;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.CodeTypeTables
{
    [AbpAuthorize(AppPermissions.Pages_CampaignTypes)]
    public class CampaignTypesAppService : RMSAppServiceBase, ICampaignTypesAppService
    {
        private readonly IRepository<CampaignType, long> _campaignTypeRepository;
        private readonly ICampaignTypesExcelExporter _campaignTypesExcelExporter;


        public CampaignTypesAppService(IRepository<CampaignType, long> campaignTypeRepository, ICampaignTypesExcelExporter campaignTypesExcelExporter)
        {
            _campaignTypeRepository = campaignTypeRepository;
            _campaignTypesExcelExporter = campaignTypesExcelExporter;

        }

        public async Task<GetCampaignTypeForViewDto> GetByCode(string code)
        {
            var campaignType =
                await _campaignTypeRepository.SingleAsync(x => x.Code == code);

            return new GetCampaignTypeForViewDto()
            {
                CampaignType = new CampaignTypeDto()
                {
                    Id = campaignType.Id,
                    Code = campaignType.Code,
                    Name = campaignType.Name
                }
            };
        }

        public async Task<PagedResultDto<GetCampaignTypeForViewDto>> GetAll(GetAllCampaignTypesInput input)
         {
			var filteredCampaignTypes = _campaignTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) );

            var pagedAndFilteredCampaignTypes = filteredCampaignTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var campaignTypes = from o in pagedAndFilteredCampaignTypes
                                select new GetCampaignTypeForViewDto()
                                {
                                    CampaignType = new CampaignTypeDto
                                    {
                                        Name = o.Name,
                                        IsActive = o.IsActive,
                                        Id = o.Id
                                    }
                                };

            var totalCount = await filteredCampaignTypes.CountAsync();

            return new PagedResultDto<GetCampaignTypeForViewDto>(
                totalCount,
                await campaignTypes.ToListAsync()
            );
        }

        public async Task<IEnumerable<GetCampaignTypeForViewDto>> GetAllWithoutPaging()
        {

            var campaignTypes = _campaignTypeRepository.GetAll().OrderBy(x => x.Name)
                        .Select(x => new GetCampaignTypeForViewDto
                        {
                            CampaignType = new CampaignTypeDto { Id = x.Id, Name = x.Name }
                        });

            return await campaignTypes.ToListAsync();
        }

        public async Task<GetCampaignTypeForViewDto> GetCampaignTypeForView(long id)
        {
            var campaignType = await _campaignTypeRepository.GetAsync(id);

            var output = new GetCampaignTypeForViewDto { CampaignType = ObjectMapper.Map<CampaignTypeDto>(campaignType) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CampaignTypes_Edit)]
        public async Task<GetCampaignTypeForEditOutput> GetCampaignTypeForEdit(EntityDto<long> input)
        {
            var campaignType = await _campaignTypeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCampaignTypeForEditOutput { CampaignType = ObjectMapper.Map<CreateOrEditCampaignTypeDto>(campaignType) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCampaignTypeDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_CampaignTypes_Create)]
        protected virtual async Task Create(CreateOrEditCampaignTypeDto input)
        {
            var campaignType = ObjectMapper.Map<CampaignType>(input);


            if (AbpSession.TenantId != null)
            {
                campaignType.TenantId = (int?)AbpSession.TenantId;
            }


            await _campaignTypeRepository.InsertAsync(campaignType);
        }

        [AbpAuthorize(AppPermissions.Pages_CampaignTypes_Edit)]
        protected virtual async Task Update(CreateOrEditCampaignTypeDto input)
        {
            var campaignType = await _campaignTypeRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, campaignType);
        }

        [AbpAuthorize(AppPermissions.Pages_CampaignTypes_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _campaignTypeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCampaignTypesToExcel(GetAllCampaignTypesForExcelInput input)
        {

            var filteredCampaignTypes = _campaignTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredCampaignTypes
                         select new GetCampaignTypeForViewDto()
                         {
                             CampaignType = new CampaignTypeDto
                             {
                                 Name = o.Name,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });


            var campaignTypeListDtos = await query.ToListAsync();

            return _campaignTypesExcelExporter.ExportToFile(campaignTypeListDtos);
        }

		public async Task<PagedResultDto<GetCampaignTypeForViewDto>> GetAllActiveCampaignType()
		{
			var filteredCampaignTypes = _campaignTypeRepository.GetAll()
				.Where(entity => entity.IsActive == true);

			var campaignTypes = from o in filteredCampaignTypes
								select new GetCampaignTypeForViewDto()
								{
									CampaignType = new CampaignTypeDto
									{
										Name = o.Name,
										IsActive = o.IsActive,
										Id = o.Id
									}
								};

			var totalCount = await filteredCampaignTypes.CountAsync();

			return new PagedResultDto<GetCampaignTypeForViewDto>(
				totalCount,
				await campaignTypes.ToListAsync()
			);
		}
	}
}