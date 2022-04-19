using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.Companies;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.Company;
using RMS.SBJ.Company.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Companies)]
    public class CompaniesController : RMSControllerBase
    {
        private readonly ICompaniesAppService _companiesAppService;

        public CompaniesController(ICompaniesAppService companiesAppService)
        {
            _companiesAppService = companiesAppService;
        }

        public ActionResult Index()
        {
            var model = new CompaniesViewModel
			{
				FilterText = ""
			};

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Companies_Create, AppPermissions.Pages_Companies_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetCompanyForEditOutput getCompanyForEditOutput;

			if (id.HasValue){
				getCompanyForEditOutput = await _companiesAppService.GetCompanyForEdit(new EntityDto<long> { Id = (long) id });
			}
			else{
				getCompanyForEditOutput = new GetCompanyForEditOutput{
					Company = new CreateOrEditCompanyDto()
				};
			}

            var viewModel = new CreateOrEditCompanyModalViewModel()
            {
				Company = getCompanyForEditOutput.Company,
					AddressPostalCode = getCompanyForEditOutput.AddressPostalCode
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewCompanyModal(long id)
        {
			var getCompanyForViewDto = await _companiesAppService.GetCompanyForView(id);

            var model = new CompanyViewModel()
            {
				Company = getCompanyForViewDto.Company
, AddressPostalCode = getCompanyForViewDto.AddressPostalCode 

            };

            return PartialView("_ViewCompanyModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Companies_Create, AppPermissions.Pages_Companies_Edit)]
        public PartialViewResult AddressLookupTableModal(long? id, string displayName)
        {
            var viewModel = new CompanyAddressLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_CompanyAddressLookupTableModal", viewModel);
        }

    }
}