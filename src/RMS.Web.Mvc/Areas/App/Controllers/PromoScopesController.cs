using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.PromoScopes;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.PromoPlanner;
using RMS.PromoPlanner.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_PromoScopes)]
    public class PromoScopesController : RMSControllerBase
    {
        private readonly IPromoScopesAppService _promoScopesAppService;

        public PromoScopesController(IPromoScopesAppService promoScopesAppService)
        {
            _promoScopesAppService = promoScopesAppService;
        }

        public ActionResult Index()
        {
            var model = new PromoScopesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_PromoScopes_Create, AppPermissions.Pages_PromoScopes_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetPromoScopeForEditOutput getPromoScopeForEditOutput;

				if (id.HasValue){
					getPromoScopeForEditOutput = await _promoScopesAppService.GetPromoScopeForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getPromoScopeForEditOutput = new GetPromoScopeForEditOutput{
						PromoScope = new CreateOrEditPromoScopeDto()
					};
				}

				var viewModel = new CreateOrEditPromoScopeModalViewModel()
				{
					PromoScope = getPromoScopeForEditOutput.PromoScope,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewPromoScopeModal(long id)
        {
			var getPromoScopeForViewDto = await _promoScopesAppService.GetPromoScopeForView(id);

            var model = new PromoScopeViewModel()
            {
                PromoScope = getPromoScopeForViewDto.PromoScope
            };

            return PartialView("_ViewPromoScopeModal", model);
        }


    }
}