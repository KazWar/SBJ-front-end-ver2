using Abp.AspNetCore.Mvc.Authorization;
using Abp.Runtime.Session;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Rest;
using Newtonsoft.Json.Linq;
using RMS.SBJ.Helpers;
using RMS.Web.Controllers;
using RMS.Web.Models.PowerBI;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize]
    public class WelcomeController : RMSControllerBase
    {
        //public ActionResult Index()
        //{
        //    return View();
        //}

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
        public async Task<IActionResult> Index([FromServices] PowerBISettingsModel powerBISettingsModel)
        {

            var tenantId = (long) AbpSession.TenantId;
            var powerBISettings = powerBISettingsModel.RMSDashBoard;

            switch (tenantId)
            {
                case TenantHelper.MakitaLive:
                    powerBISettings = powerBISettingsModel.MakitaDashBoard;
                    break;

                case TenantHelper.MakitaTest:
                    powerBISettings = powerBISettingsModel.MakitaTestDashBoard;
                    break;

                case TenantHelper.WeberLive:
                    powerBISettings = powerBISettingsModel.WeberDashBoard;
                    break;

                case TenantHelper.SageLive:
                    powerBISettings = powerBISettingsModel.SageDashBoard;
                    break;

                case TenantHelper.BatLive:
                    powerBISettings = powerBISettingsModel.BatDashBoard;
                    break;

                case TenantHelper.SanimedLive:
                    powerBISettings = powerBISettingsModel.SanimedDashBoard;
                    break;

                case TenantHelper.WhirlpoolLive:
                    powerBISettings = powerBISettingsModel.WhirlpoolDashBoard;
                    break;

                case TenantHelper.CarocrocLive:
                    powerBISettings = powerBISettingsModel.CarocrocDashBoard;
                    break;

                case TenantHelper.MakitaBELive:
                    powerBISettings = powerBISettingsModel.MakitaBEDashboard;
                    break;

                case TenantHelper.PhilipsPHLive:
                    powerBISettings = powerBISettingsModel.PhilipsPHDashboard;
                    break;

                case TenantHelper.PhilipsDaLive:
                    powerBISettings = powerBISettingsModel.PhilipsDADashboard;
                    break;

                default:
                    return Redirect("Registrations");
            };

            var result = new EmbedConfig { Username = powerBISettings.UserName };
            var accessToken = await GetPowerBIAccessToken(powerBISettings);

            var tokenCredentials = new TokenCredentials(accessToken, "Bearer");
            try
            {
                using (var client = new PowerBIClient(new Uri(powerBISettings.ApiUrl), tokenCredentials))
                {
                    var workspaceId = powerBISettings.WorkspaceId;
                    var reportId = powerBISettings.ReportId;
                    var report = await client.Reports.GetReportInGroupAsync((Guid)workspaceId, reportId);

                    var dataset = await client.Datasets.GetDatasetAsync((Guid)workspaceId, report.DatasetId);
                    result.IsEffectiveIdentityRequired = dataset.IsEffectiveIdentityRolesRequired;
                    result.IsEffectiveIdentityRolesRequired = dataset.IsEffectiveIdentityRolesRequired;


                    var generateTokenRequestParameters = new GenerateTokenRequest(accessLevel: "view");
                    var tokenResponse = await client.Reports.GenerateTokenInGroupAsync((Guid)workspaceId, reportId, generateTokenRequestParameters);


                    result.EmbedToken = tokenResponse;
                    result.EmbedUrl = $"https://app.powerbi.com/reportEmbed?reportId={reportId.ToString()}&groupId={workspaceId}";
                    result.Id = report.Id.ToString();
                }
                return View(result);
            }
            catch
            {
                return Redirect("Registrations");
            }

        }
    }
}