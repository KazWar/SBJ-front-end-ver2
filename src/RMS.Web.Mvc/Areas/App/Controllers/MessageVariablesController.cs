using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Areas.App.Models.MessageVariables;
using RMS.Web.Controllers;
using RMS.Authorization;
using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.CodeTypeTables.Dtos;
using Abp.Application.Services.Dto;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_MessageVariables)]
    public class MessageVariablesController : RMSControllerBase
    {
        private readonly IMessageVariablesAppService _messageVariablesAppService;

        public MessageVariablesController(IMessageVariablesAppService messageVariablesAppService)
        {
            _messageVariablesAppService = messageVariablesAppService;
        }

        public ActionResult Index()
        {
            var model = new MessageVariablesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }
        
        public async Task<PagedResultDto<GetMessageVariableForViewDto>> GetAll()
        {
            return await _messageVariablesAppService.GetAll(new GetAllMessageVariablesInput { Filter = "" });
        }

        [AbpMvcAuthorize(AppPermissions.Pages_MessageVariables_Create, AppPermissions.Pages_MessageVariables_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
            GetMessageVariableForEditOutput getMessageVariableForEditOutput;

            if (id.HasValue)
            {
                getMessageVariableForEditOutput = await _messageVariablesAppService.GetMessageVariableForEdit(new EntityDto<long> { Id = (long)id });
            }
            else
            {
                getMessageVariableForEditOutput = new GetMessageVariableForEditOutput
                {
                    MessageVariable = new CreateOrEditMessageVariableDto()
                };
            }

            var viewModel = new CreateOrEditMessageVariableModalViewModel()
            {
                MessageVariable = getMessageVariableForEditOutput.MessageVariable,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewMessageVariableModal(long id)
        {
            var getMessageVariableForViewDto = await _messageVariablesAppService.GetMessageVariableForView(id);

            var model = new MessageVariableViewModel()
            {
                MessageVariable = getMessageVariableForViewDto.MessageVariable
            };

            return PartialView("_ViewMessageVariableModal", model);
        }
    }
}