using System.Collections.Generic;

namespace RMS.Web.Website.PhilipsPH.Configuration
{
    public sealed class AuthenticationConfiguration
    {
        public string UserNameOrEmailAddress { get; set; }

        public string Password { get; set; }

        public Dictionary<string, JwtBearerInnerConfiguration> JwtBearer { get; set; }
    }

    public sealed class JwtBearerInnerConfiguration
    {
        public string IsEnabled { get; set; }

        public string SecurityKey { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
    }

    public class AuthModel
    {
        public ResultModel result { get; set; }
    }

    public class ResultModel
    {
        public string accessToken { get; set; }

    }

    public class GetToken
    {
        public FirstResult Result { get; set; }
    }

    public class FirstResult
    {
        public string Value { get; set; }
    }

    public sealed class ApiCalls
    {
        public Result Result { get; set; }
        public string TargetUrl { get; set; }
        public string Succes { get; set; }
        public string Error { get; set; }
        public string UnAuthorizedRequest { get; set; }
        public string __abp { get; set; }
    }

    public sealed class Result
    {
        public string Version { get; set; }
        public string Content { get; set; }
        public string statusCode { get; set; }
        public string ReasonPhrase { get; set; }
    }

    public sealed class PostCodeApi
    {
        public string key { get; set; }

        public string secret { get; set; }
    }
}
