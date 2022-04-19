using RMS.SBJ.Company;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.Company.Exporting;
using RMS.SBJ.Company.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.Company
{
	[AbpAuthorize(AppPermissions.Pages_ProjectManagers)]
    public class ProjectManagersAppService : RMSAppServiceBase, IProjectManagersAppService
    {
		 private readonly IRepository<ProjectManager, long> _projectManagerRepository;
		 private readonly IProjectManagersExcelExporter _projectManagersExcelExporter;
		 private readonly IRepository<Address,long> _lookup_addressRepository;
		 

		  public ProjectManagersAppService(IRepository<ProjectManager, long> projectManagerRepository, IProjectManagersExcelExporter projectManagersExcelExporter , IRepository<Address, long> lookup_addressRepository) 
		  {
			_projectManagerRepository = projectManagerRepository;
			_projectManagersExcelExporter = projectManagersExcelExporter;
			_lookup_addressRepository = lookup_addressRepository;
		
		  }

		 public async Task<PagedResultDto<GetProjectManagerForViewDto>> GetAll(GetAllProjectManagersInput input)
         {
			
			var filteredProjectManagers = _projectManagerRepository.GetAll()
						.Include( e => e.AddressFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.PhoneNumber.Contains(input.Filter) || e.EmailAddress.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PhoneNumberFilter),  e => e.PhoneNumber == input.PhoneNumberFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.EmailAddressFilter),  e => e.EmailAddress == input.EmailAddressFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AddressPostalCodeFilter), e => e.AddressFk != null && e.AddressFk.PostalCode == input.AddressPostalCodeFilter);

			var pagedAndFilteredProjectManagers = filteredProjectManagers
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var projectManagers = from o in pagedAndFilteredProjectManagers
                         join o1 in _lookup_addressRepository.GetAll() on o.AddressId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetProjectManagerForViewDto() {
							ProjectManager = new ProjectManagerDto
							{
                                Name = o.Name,
                                PhoneNumber = o.PhoneNumber,
                                EmailAddress = o.EmailAddress,
                                Id = o.Id
							},
                         	AddressPostalCode = s1 == null ? "" : s1.PostalCode.ToString()
						};

            var totalCount = await filteredProjectManagers.CountAsync();

            return new PagedResultDto<GetProjectManagerForViewDto>(
                totalCount,
                await projectManagers.ToListAsync()
            );
         }
		 
		 public async Task<GetProjectManagerForViewDto> GetProjectManagerForView(long id)
         {
            var projectManager = await _projectManagerRepository.GetAsync(id);

            var output = new GetProjectManagerForViewDto { ProjectManager = ObjectMapper.Map<ProjectManagerDto>(projectManager) };

		    if (output.ProjectManager.AddressId != null)
            {
                var _lookupAddress = await _lookup_addressRepository.FirstOrDefaultAsync((long)output.ProjectManager.AddressId);
                output.AddressPostalCode = _lookupAddress.PostalCode.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ProjectManagers_Edit)]
		 public async Task<GetProjectManagerForEditOutput> GetProjectManagerForEdit(EntityDto<long> input)
         {
            var projectManager = await _projectManagerRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetProjectManagerForEditOutput {ProjectManager = ObjectMapper.Map<CreateOrEditProjectManagerDto>(projectManager)};

		    if (output.ProjectManager.AddressId != null)
            {
                var _lookupAddress = await _lookup_addressRepository.FirstOrDefaultAsync((long)output.ProjectManager.AddressId);
                output.AddressPostalCode = _lookupAddress.PostalCode.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditProjectManagerDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ProjectManagers_Create)]
		 protected virtual async Task Create(CreateOrEditProjectManagerDto input)
         {
            var projectManager = ObjectMapper.Map<ProjectManager>(input);

			
			if (AbpSession.TenantId != null)
			{
				projectManager.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _projectManagerRepository.InsertAsync(projectManager);
         }

		 [AbpAuthorize(AppPermissions.Pages_ProjectManagers_Edit)]
		 protected virtual async Task Update(CreateOrEditProjectManagerDto input)
         {
            var projectManager = await _projectManagerRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, projectManager);
         }

		 [AbpAuthorize(AppPermissions.Pages_ProjectManagers_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _projectManagerRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetProjectManagersToExcel(GetAllProjectManagersForExcelInput input)
         {
			
			var filteredProjectManagers = _projectManagerRepository.GetAll()
						.Include( e => e.AddressFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.PhoneNumber.Contains(input.Filter) || e.EmailAddress.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PhoneNumberFilter),  e => e.PhoneNumber == input.PhoneNumberFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.EmailAddressFilter),  e => e.EmailAddress == input.EmailAddressFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AddressPostalCodeFilter), e => e.AddressFk != null && e.AddressFk.PostalCode == input.AddressPostalCodeFilter);

			var query = (from o in filteredProjectManagers
                         join o1 in _lookup_addressRepository.GetAll() on o.AddressId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetProjectManagerForViewDto() { 
							ProjectManager = new ProjectManagerDto
							{
                                Name = o.Name,
                                PhoneNumber = o.PhoneNumber,
                                EmailAddress = o.EmailAddress,
                                Id = o.Id
							},
                         	AddressPostalCode = s1 == null ? "" : s1.PostalCode.ToString()
						 });


            var projectManagerListDtos = await query.ToListAsync();

            return _projectManagersExcelExporter.ExportToFile(projectManagerListDtos);
         }


    }
}