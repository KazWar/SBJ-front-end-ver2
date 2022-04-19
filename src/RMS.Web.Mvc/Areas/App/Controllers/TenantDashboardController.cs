using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Rest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RMS.Authorization;
using RMS.DashboardCustomization;
using RMS.Tenants.Dashboard;
using RMS.Web.DashboardCustomization;
using RMS.Web.Models.PowerBI;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Dashboard)]
    public class TenantDashboardController : CustomizableDashboardControllerBase
    {
        private readonly ITenantDashboardAppService _tenantDashboardAppService;
        public TenantDashboardController(DashboardViewConfiguration dashboardViewConfiguration, 
            IDashboardCustomizationAppService dashboardCustomizationAppService, ITenantDashboardAppService tenantDashboardAppService) 
            : base(dashboardViewConfiguration, dashboardCustomizationAppService)
        {
            _tenantDashboardAppService = tenantDashboardAppService;
        }


        private async Task<string> GetPowerBIAccessToken(PowerBiSettings powerBISettings)
        {
            using (var client = new HttpClient())
            {
                var form = new Dictionary<string, string>();
                form["grant_type"] = "password";
                form["resource"] = powerBISettings.ResourceUrl;
                form["username"] = powerBISettings.UserName;
                form["password"] = powerBISettings.Password;
                form["client_id"] = powerBISettings.ApplicationId.ToString();
                form["client_secret"] = powerBISettings.ApplicationSecret;
                form["scope"] = "openid";

                var authorityUrl = powerBISettings.AuthorityUrl.Replace("common", powerBISettings.TenantId); // Make it Tenant specific
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");

                using (var formContent = new FormUrlEncodedContent(form))
                using (var response = await client.PostAsync(authorityUrl, formContent))
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var jsonBody = JObject.Parse(body);

                    var errorToken = jsonBody.SelectToken("error");
                    if (errorToken != null)
                    {
                        throw new Exception(errorToken.Value<string>());
                    }

                    return jsonBody.SelectToken("access_token").Value<string>();
                }
            }
        }


        [DontWrapResult]
        [HttpGet]
        public async Task<JsonResult> GetDashboardTiles([FromServices] PowerBISettingsModel powerBISettingsModel)
        {
            var tenantId = AbpSession.TenantId;
            var powerBISettings = powerBISettingsModel.RMSDashBoard;
            if (tenantId == null)
            {
                return null;
            }
            else if (tenantId == 8)
            {
                powerBISettings = powerBISettingsModel.MakitaDashBoard;
            }
            else if (tenantId == 32)
            {
                powerBISettings = powerBISettingsModel.MakitaTestDashBoard;
            };


            var accessToken = await GetPowerBIAccessToken(powerBISettings);
            var tokenCredentials = new TokenCredentials(accessToken, "Bearer");
            var configList = new List<EmbedConfig>();

            using (var client = new PowerBIClient(new Uri(powerBISettings.ApiUrl), tokenCredentials))
            {

                var workspaceId = powerBISettings.WorkspaceId;
                var dashboardId = powerBISettings.DashboardId;
                var generateTokenRequestParameters = new GenerateTokenRequest(accessLevel: "view");
                var tiles = await client.Dashboards.GetTilesInGroupAsync((Guid)workspaceId, dashboardId);

                foreach (var dataTile in tiles.Value)
                {
                    var result = new EmbedConfig { Username = powerBISettings.UserName };
                    var tokenResponse = await client.Tiles.GenerateTokenInGroupAsync((Guid)workspaceId, dashboardId, dataTile.Id, generateTokenRequestParameters);
                    result.EmbedToken = tokenResponse;
                    result.EmbedUrl = $"https://app.powerbi.com/embed?dashboardId={dashboardId.ToString()}&tileId={dataTile.Id.ToString()}";
                    result.Id = dataTile.Id.ToString();
                    result.Title = dataTile.Title.Trim().ToUpper();
                    result.DashboardId = dashboardId.ToString();
                    configList.Add(result);
                }
            }
            var listings = JsonConvert.SerializeObject(configList);
            return Json(listings);
        }


        public async Task<ActionResult> Index()
        {
            return await GetView(RMSDashboardCustomizationConsts.DashboardNames.DefaultTenantDashboard);
        }

    }
}