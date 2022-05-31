namespace RMS.Web.Shared.Controllers
{
    [Route("api/v2")]
    public abstract class ApiController : ControllerBase
    {
        // These configurations MUST be supplied by the websites themselves.
        public BuildConfiguration BuildConfiguration { get; set; }
        public AuthenticationConfiguration AuthenticationConfiguration { get; set; }
        public TenantConfiguration TenantConfiguration { get; set; }

        // Shared configurations properties
        private readonly ApiConfiguration ApiConfig;

        // Define the REST clients
        private RestClient Client = null!;
        private RestClient TokenClient = null!;

        /// <summary>
        /// Creates an instance of the shared API controller.
        /// </summary>
        /// <param name="authenticationConfiguration"></param>
        /// <param name="tenantConfiguration"></param>
        /// <param name="buildConfiguration"></param>
        public ApiController(
                IOptions<AuthenticationConfiguration> authenticationConfiguration,
                IOptions<TenantConfiguration> tenantConfiguration,
                IOptions<BuildConfiguration> buildConfiguration
            )
        {
            // Import the appsettings resource
            byte[] AppsettingsResource = Properties.Resources.appsettings;

            // Convert byte array to stream for the configuration builder
            MemoryStream stream = new(AppsettingsResource);

            // Create an app configuration object from the stream.
            var Configuration = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            // These configurations must be provided from the website specific appsettings.
            TenantConfiguration = tenantConfiguration.Value;
            AuthenticationConfiguration = authenticationConfiguration.Value;
            BuildConfiguration = buildConfiguration.Value;

            // These configurations are taken from the shared appsettings.
            ApiConfig = Configuration.GetSection("Api").Get<ApiConfiguration>();

            Console.WriteLine("Starting app");
            Init();
        }

        /// <summary>
        /// Initializes client instances for the api to use
        /// </summary>
        public virtual void Init()
        {
            // Get the urls from the appsettings.json
            // BaseUrl is based of the environment flag set in the appsettings build configuration.
            var baseUrl = ApiConfig.ApiRootAddresses[BuildConfiguration.Environment];
            var tokenURL = "https://app-sbj-rms2.azurewebsites.net/api";

            // First iniate the token Client as it will be used by the general api Client to get a token.
            TokenClient = new RestClient(tokenURL)
                .AddDefaultHeader("Abp.TenantId", TenantConfiguration.Id.ToString())
                .AddDefaultHeader("Content-Type", "application/json");

            // Create default Client options.
            var options = new RestClientOptions(baseUrl);

            // Create a default Client instance to handle all API calls.
            Client = new RestClient(options)
                    .AddDefaultHeader("Abp.TenantId", TenantConfiguration.Id.ToString())
                    .AddDefaultHeader("Content-Type", "application/json")
                    .UseAuthenticator(new JwtAuthenticator(UnpackBearerToken(GetToken())));
        }

        #region API call functions

        /// <summary>
        /// Calls the authentication API url and gets a response.
        /// The bearer token is then returned.
        /// </summary>
        /// <returns>A RestResponse object containing token data</returns>
        public virtual async Task<RestResponse> GetToken()
        {
            // Use the special token client instance.
            return await TokenClient.PostAsync(
                Request("TokenAuth/Authenticate")
                .AddJsonBody(
                    // Get the appsettings username & password
                    new
                    {
                        userNameOrEmailAddress = AuthenticationConfiguration.UserNameOrEmailAddress,
                        password = AuthenticationConfiguration.Password
                    })
                );
        }


        
        [HttpGet]
        [Route("locale")]
        public async Task<RestResponse> Locale()
        {
            return await Client.GetAsync(Request("locale"));
        }

        [HttpGet]
        [Route("campaign")]
        public async Task<RestResponse> Campaign(
            [FromQuery] string code)
        {
            return await Client.GetAsync(
                Request("campaign")
                .AddQueryParameter("locale", code));
        }

        [AcceptVerbs("GET", "POST", "PUT")]
        [Route("locale/{code:maxlength(5)}/campaign/{cid:int}/registration/{rid:int?}")]
        public async Task<RestResponse> Form(
            // There are actually 2 DTO's for creating & update, FormRegistrationHandlingDto and UpdateRegistrationDataDto respectively
            // But it's not possible to have 2 [FromBody] attributes as the first one 'consumes' it
            // However FormRegistrationHandlingDto has everything UpdateRegistrationDataDto has
            [FromBody] FormRegistrationHandlingDto vueJsToRmsModel,
            [FromRoute] int rid,
            [FromRoute] string code,
            [FromQuery] string password)
        {
            var method = "";

            if (ModelState.IsValid) { 
                // Extract the used method from the request
                method = HttpContext.Request.Method;
            }

            string data = JsonConvert.SerializeObject(vueJsToRmsModel);

            // Match & execute matching api call to the RMS
            return method switch
            {
                // This entire internal api is crap. Update, send and get for registration with 3 seperate addresses.
                // it should all resolve to /registration and then depending on the method either create, update or get the registration.
                // But this is the best way to do it now.
                "GET" => await Client.PostAsync(
                    Request("Registrations/GetEditForRegistration", Method.Post)
                    .AddJsonBody(
                        new GetFormLayoutAndDataInput
                        {
                            RegistrationId = rid,
                            Password = password,
                            Locale = code
                        })
                    ),

                "POST" => await Client.PostAsync(
                    Request("Registrations/SendFormData", Method.Post)
                    .AddJsonBody(
                        new FormRegistrationHandlingDto {
                            Data = data
                        }
                    )),

                "PUT" => await Client.PutAsync(
                    Request("Registrations/UpdateFormData", Method.Put)
                    .AddJsonBody(
                        new UpdateRegistrationDataDto {
                            Data = vueJsToRmsModel.Data
                        })
                    ),

                    //Default case
                _ => throw new ArgumentOutOfRangeException(),// Replace with a a better error.
            };
        }

        [HttpGet]
        [Route("locale/{code:maxlength(5)}/campaign/{id:int}/form")]
        public async Task<RestResponse> FormAndProductHandling(
            [FromRoute] string code,
            [FromRoute] int id)
        {
            return await Client.GetAsync(
                Request("FormLocales/GetFormAndProductHandlingApi")
                .AddQueryParameter("currentLocale", code)
                .AddQueryParameter("currentCampaignId", id));
        }

        #endregion

        #region Helper functions

        /// <summary>
        /// Creates a RestRequest object from apiPath and method.
        /// </summary>
        /// <param name="apiPath"> Path to the api resource</param>
        /// <param name="method"> HTTP request method, defaults to GET</param>
        /// <returns> RestRequest object</returns>
        private new RestRequest Request(string apiPath, Method method = Method.Get)
        {
            return new RestRequest(apiPath, method);
        }

        /// <summary>
        /// Takes in the JSON response from the token API, parses it and returns the bearer token
        /// </summary>
        /// <param name="response"></param>
        /// <returns>A string representation of the bearer token</returns>
        private static string UnpackBearerToken(Task<RestResponse> response)
        {
            var content = response.Result.Content!.ToString() ?? null;

            if (response is not null)
            {
                var tokenResponse = JsonConvert.DeserializeObject<AuthModel>(content!);
                return tokenResponse!.result.accessToken;
            } else { 
                throw new ArgumentNullException(content);
            }
        }
        #endregion
    }
}