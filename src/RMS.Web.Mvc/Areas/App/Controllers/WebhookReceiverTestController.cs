using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace RMS.Web.Areas.App.Controllers
{
	[Area("App")]
	//[ApiController]
	public class WebhookReceiverTestController : RMSControllerBase
	{
		[System.Web.Mvc.HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> WebHookTest()
		{
			using (StreamReader reader = new StreamReader(HttpContext.Request.Body, Encoding.UTF8))
			{
				var body = await reader.ReadToEndAsync();

				if (!IsSignatureCompatible("whs_66bda4a1fb204190b85fd39b418e50e8", body))//read webhooksecret from user secret
				{
					return BadRequest("Unexpected Signature");
					//throw new Exception("Unexpected Signature");
				}
				//It is certain that Webhook has not been modified.
				return Ok("Webhook received");
			}
		}

		private bool IsSignatureCompatible(string secret, string body)
		{
			if (!HttpContext.Request.Headers.ContainsKey("abp-webhook-signature"))
			{
				return false;
			}

			var receivedSignature = HttpContext.Request.Headers["abp-webhook-signature"].ToString().Split("=");//will be something like "sha256=whs_XXXXXXXXXXXXXX"
																											   //It starts with hash method name (currently "sha256") then continue with signature. You can also check if your hash method is true.

			string computedSignature;
			switch (receivedSignature[0])
			{
				case "sha256":
					var secretBytes = Encoding.UTF8.GetBytes(secret);
					using (var hasher = new HMACSHA256(secretBytes))
					{
						var data = Encoding.UTF8.GetBytes(body);
						computedSignature = BitConverter.ToString(hasher.ComputeHash(data));
					}
					break;
				default:
					throw new NotImplementedException();
			}
			return computedSignature == receivedSignature[1];
		}
	}
}
