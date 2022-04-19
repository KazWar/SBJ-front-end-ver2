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
	[AbpAuthorize(AppPermissions.Pages_PromoScopes)]
    public class PromoScopesAppService : RMSAppServiceBase, IPromoScopesAppService
    {
		 private readonly IRepository<PromoScope, long> _promoScopeRepository;
		 private readonly IPromoScopesExcelExporter _promoScopesExcelExporter;
		 

		  public PromoScopesAppService(IRepository<PromoScope, long> promoScopeRepository, IPromoScopesExcelExporter promoScopesExcelExporter ) 
		  {
			_promoScopeRepository = promoScopeRepository;
			_promoScopesExcelExporter = promoScopesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetPromoScopeForViewDto>> GetAll(GetAllPromoScopesInput input)
         {
			
			var filteredPromoScopes = _promoScopeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter);

			var pagedAndFilteredPromoScopes = filteredPromoScopes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var promoScopes = from o in pagedAndFilteredPromoScopes
                         select new GetPromoScopeForViewDto() {
							PromoScope = new PromoScopeDto
							{
                                Description = o.Description,
                                Id = o.Id
							}
						};

            var totalCount = await filteredPromoScopes.CountAsync();

            return new PagedResultDto<GetPromoScopeForViewDto>(
                totalCount,
                await promoScopes.ToListAsync()
            );
         }
		 
		 public async Task<IEnumerable<GetPromoScopeForViewDto>> GetAllWithoutPaging()
		 {

			var promoScopes = _promoScopeRepository.GetAll().OrderBy(x => x.Description)
						.Select(x => new GetPromoScopeForViewDto
						{
							PromoScope = new PromoScopeDto { Id = x.Id, Description = x.Description }
						});

			return await promoScopes.ToListAsync();
		 }

		public async Task<GetPromoScopeForViewDto> GetPromoScopeForView(long id)
         {
            var promoScope = await _promoScopeRepository.GetAsync(id);

            var output = new GetPromoScopeForViewDto { PromoScope = ObjectMapper.Map<PromoScopeDto>(promoScope) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_PromoScopes_Edit)]
		 public async Task<GetPromoScopeForEditOutput> GetPromoScopeForEdit(EntityDto<long> input)
         {
            var promoScope = await _promoScopeRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetPromoScopeForEditOutput {PromoScope = ObjectMapper.Map<CreateOrEditPromoScopeDto>(promoScope)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditPromoScopeDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_PromoScopes_Create)]
		 protected virtual async Task Create(CreateOrEditPromoScopeDto input)
         {
            var promoScope = ObjectMapper.Map<PromoScope>(input);

			
			if (AbpSession.TenantId != null)
			{
				promoScope.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _promoScopeRepository.InsertAsync(promoScope);
         }

		 [AbpAuthorize(AppPermissions.Pages_PromoScopes_Edit)]
		 protected virtual async Task Update(CreateOrEditPromoScopeDto input)
         {
            var promoScope = await _promoScopeRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, promoScope);
         }

		 [AbpAuthorize(AppPermissions.Pages_PromoScopes_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _promoScopeRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetPromoScopesToExcel(GetAllPromoScopesForExcelInput input)
         {
			
			var filteredPromoScopes = _promoScopeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter);

			var query = (from o in filteredPromoScopes
                         select new GetPromoScopeForViewDto() { 
							PromoScope = new PromoScopeDto
							{
                                Description = o.Description,
                                Id = o.Id
							}
						 });


            var promoScopeListDtos = await query.ToListAsync();

            return _promoScopesExcelExporter.ExportToFile(promoScopeListDtos);
         }


    }
}