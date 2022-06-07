using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Stripe;
using Hangfire;
using Owl.reCAPTCHA;
using Castle.Facilities.Logging;
using IdentityServer4.Configuration;
using Abp.Hangfire;
using Abp.PlugIns;
using Abp.AspNetCore;
using Abp.AspNetCore.SignalR.Hubs;
using Abp.AspNetCore.Mvc.Antiforgery;
using Abp.AspNetZeroCore.Web.Authentication.JwtBearer;
using Abp.Castle.Logging.Log4Net;
using RMS.Authorization;
using RMS.Configuration;
using RMS.EntityFrameworkCore;
using RMS.Identity;
using RMS.Web.Chat.SignalR;
using RMS.Web.Common;
using RMS.Web.Resources;
using RMS.Web.IdentityServer;
using RMS.Web.Swagger;
using RMS.Web.HealthCheck;
using RMS.Web.Models.PowerBI;
using RMS.Web.Models.TenantSetup;
using HealthChecks.UI.Client;
using HealthChecksUISettings = HealthChecks.UI.Configuration.Settings;
using RMS.SBJ.Makita;
using RMS.Web.Models.AzureBlobStorageForMessages;
using Microsoft.EntityFrameworkCore;
using RMS.Web.Models.AzureBlobStorage;
using RMS.SBJ.HandlingBatch;
using RMS.External;

namespace RMS.Web.Startup
{
    public class Startup
    {
        private const string CorsPolicyName = "AllowFrontEndClientOnly";
        private readonly string[] AllowedOrigins;

        private readonly IConfigurationRoot _appConfiguration;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public Startup(IWebHostEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
            _hostingEnvironment = env;

            AllowedOrigins = new string[] { "http://localhost:44302", "https://localhost:44302", "http://localhost:44303", "https://localhost:44303"};
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<AzureBlobStorageSettingsModel>(_appConfiguration.GetSection("AzureBlobStorageSettingsForMessages"));
            // MVC
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AbpAutoValidateAntiforgeryTokenAttribute());
            })
#if DEBUG
                .AddRazorRuntimeCompilation()
#endif
                .AddNewtonsoftJson();

            IdentityRegistrar.Register(services);

            //Identity server
            if (bool.Parse(_appConfiguration["IdentityServer:IsEnabled"]))
            {
                IdentityServerRegistrar.Register(services, _appConfiguration, options =>
                    options.UserInteraction = new UserInteractionOptions()
                    {
                        LoginUrl = "/Account/Login",
                        LogoutUrl = "/Account/LogOut",
                        ErrorUrl = "/Error"
                    });
            }

            services.AddCors(options => {
                options.AddPolicy(
                    name: CorsPolicyName,
                    builder => { 
                        builder
                            .WithOrigins(AllowedOrigins)
                            .SetIsOriginAllowedToAllowWildcardSubdomains()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    }
                );
            });

            AuthConfigurer.Configure(services, _appConfiguration);

            if (WebConsts.SwaggerUiEnabled)
            {
                //Swagger - Enable this line and the related lines in Configure method to enable swagger UI
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo() { Title = "RMS API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.ParameterFilter<SwaggerEnumParameterFilter>();
                    options.SchemaFilter<SwaggerEnumSchemaFilter>();
                    options.OperationFilter<SwaggerOperationIdFilter>();
                    options.OperationFilter<SwaggerOperationFilter>();
                    options.CustomDefaultSchemaIdSelector();
                }).AddSwaggerGenNewtonsoftSupport();
            }

            //Recaptcha
            services.AddreCAPTCHAV3(x =>
            {
                x.SiteKey = _appConfiguration["Recaptcha:SiteKey"];
                x.SiteSecret = _appConfiguration["Recaptcha:SecretKey"];
            });

            //if (WebConsts.HangfireDashboardEnabled)
            //{
            //    //Hangfire (Enable to use Hangfire instead of default job manager)
            //    services.AddHangfire(config =>
            //    {
            //        config.UseSqlServerStorage(_appConfiguration.GetConnectionString("Default"));
            //    });
            //}

            services.AddScoped<IWebResourceManager, WebResourceManager>();
            services.AddSignalR();

            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.Zero;
            });

            if (bool.Parse(_appConfiguration["HealthChecks:HealthChecksEnabled"]))
            {
                services.AddAbpZeroHealthCheck();

                var healthCheckUISection = _appConfiguration.GetSection("HealthChecks")?.GetSection("HealthChecksUI");

                if (bool.Parse(healthCheckUISection["HealthChecksUIEnabled"]))
                {
                    services.Configure<HealthChecksUISettings>(settings =>
                    {
                        healthCheckUISection.Bind(settings, c => c.BindNonPublicProperties = true);
                    });

                    services.AddHealthChecksUI();
                }
            }

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new RazorViewLocationExpander());
            });

            
            services.AddServerSideBlazor();
            services.AddLogging(); // Add logging so that loggers can be injected into services using ILogger<T>

            //PowerBI Settings
            var powerBISettings = _appConfiguration.GetSection("PowerBI").Get<PowerBISettingsModel>();
            services.AddSingleton(powerBISettings);

            //Azure Blob Storage Settings
            //var azureBlobStorageSettings = _appConfiguration.GetSection("AzureBlobStorageSettings").Get<AzureBlobStorageSettingsModel>();
            //services.AddSingleton(azureBlobStorageSettings);

            //Tenant Setup
            var tenantSetup = _appConfiguration.GetSection("TenantSetup").Get<TenantSetupModel>();
            services.AddSingleton(tenantSetup);

            //Azure Blob Storage Settings for messages
            var azureBlobStorageSettingsMessages = _appConfiguration.GetSection("AzureBlobStorageSettingsForMessages").Get<AzureBlobSettingsMessageModel>();
            services.AddSingleton(azureBlobStorageSettingsMessages);

            //Configure Abp and Dependency Injection
            services.AddSingleton<IHandlingPremiumBatchesBackgroundJob, HandlingPremiumBatchesBackgroundJob>();
            services.AddSingleton<IHandlingCashRefundBatchesBackgroundJob, HandlingCashRefundBatchesBackgroundJob>();
            services.AddSingleton<IHandlingActivationCodeBatchesBackgroundJob, HandlingActivationCodeBatchesBackgroundJob>();
            services.AddSingleton<IScanWarehouseStatusBackgroundJob, ScanWarehouseStatusBackgroundJob>();
            services.AddSingleton<IScanActivationCodeStatusBackgroundJob, ScanActivationCodeStatusBackgroundJob>();
            services.AddSingleton<IWmsOrder, WmsOrder>();
            services.AddSingleton<IWmsProduct, WmsProduct>();
            return services.AddAbp<RMSWebMvcModule>(options =>
            {
                //Configure Log4Net logging
                options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig(_hostingEnvironment.IsDevelopment()
                        ? "log4net.config"
                        : "log4net.Production.config")
                );

                options.PlugInSources.AddFolder(Path.Combine(_hostingEnvironment.WebRootPath, "Plugins"), SearchOption.AllDirectories);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Initializes the ABP framework.
            app.UseAbp(options =>
            {
                options.UseAbpRequestLocalization = false; //used below: UseAbpRequestLocalization
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithRedirects("~/Error?statusCode={0}");
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors(CorsPolicyName);

            app.UseAuthentication();

            if (bool.Parse(_appConfiguration["Authentication:JwtBearer:IsEnabled"]))
            {
                app.UseJwtTokenMiddleware();
            }

            if (bool.Parse(_appConfiguration["IdentityServer:IsEnabled"]))
            {
                app.UseJwtTokenMiddleware("IdentityBearer");
                app.UseIdentityServer();
            }

            app.UseAuthorization();

            using (var scope = app.ApplicationServices.CreateScope())
            {
                if (scope.ServiceProvider.GetService<DatabaseCheckHelper>().Exist(_appConfiguration["ConnectionStrings:Default"]))
                {
                    app.UseAbpRequestLocalization();
                }
            }

            //if (WebConsts.HangfireDashboardEnabled)
            //{
            //    //Hangfire dashboard & server (Enable to use Hangfire instead of default job manager)
            //    //app.UseHangfireDashboard("/hangfire", new DashboardOptions
            //    //{
            //    //    Authorization = new[] { new AbpHangfireAuthorizationFilter(AppPermissions.Pages_Administration_HangfireDashboard) }
            //    //});

            //    app.UseHangfireDashboard("/hangfire");
            //    app.UseHangfireServer();
            //}

            if (bool.Parse(_appConfiguration["Payment:Stripe:IsActive"]))
            {
                StripeConfiguration.ApiKey = _appConfiguration["Payment:Stripe:SecretKey"];
            }


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<AbpCommonHub>("/signalr");
                endpoints.MapHub<ChatHub>("/signalr-chat");

                endpoints.MapControllerRoute("defaultWithArea", "{area}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

                if (bool.Parse(_appConfiguration["HealthChecks:HealthChecksEnabled"]))
                {
                    endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                    {
                        Predicate = _ => true,
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                    });
                }
            });

            if (bool.Parse(_appConfiguration["HealthChecks:HealthChecksEnabled"]))
            {
                if (bool.Parse(_appConfiguration["HealthChecks:HealthChecksUI:HealthChecksUIEnabled"]))
                {
                    app.UseHealthChecksUI();
                }
            }

            if (WebConsts.SwaggerUiEnabled)
            {
                // Enable middleware to serve generated Swagger as a JSON endpoint
                app.UseSwagger();
                //Enable middleware to serve swagger - ui assets(HTML, JS, CSS etc.)
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(_appConfiguration["App:SwaggerEndPoint"], "RMS API V1");
                    options.IndexStream = () => Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("RMS.Web.wwwroot.swagger.ui.index.html");
                    options.InjectBaseUrl(_appConfiguration["App:WebSiteRootAddress"]);
                }); //URL: /swagger
            }
        }
    }
}
