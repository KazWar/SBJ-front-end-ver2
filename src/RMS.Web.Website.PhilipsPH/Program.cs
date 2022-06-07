// Define Variables
var SPA_ROOT_PATH = "wwwroot";

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AuthenticationConfiguration>(builder.Configuration.GetSection("Authentication"));
builder.Services.Configure<TenantConfiguration>(builder.Configuration.GetSection("Tenant"));
builder.Services.Configure<BuildConfiguration>(builder.Configuration.GetSection("Build"));

// Add services to the container with endpoint routing disabled as we use Attribute routing instead.
// Bind the Newtonsoft Serializer to the services.
builder.Services
    .AddControllers(options => options.EnableEndpointRouting = false)
    .AddNewtonsoftJson();

// Configure the CORS header for requests.
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

// Add SPA static files to the container.
builder.Services.AddSpaStaticFiles(configuration: options => {
    options.RootPath = "wwwroot/dist"; 
});

// Build the container using the above settings
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");

    // The default HSTS value is 30 days. You may want to change
    // this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
} else {
    // Output exceptions during development mode.
    app.UseDeveloperExceptionPage();
}

// Add HTTP to HTTPS redirection to the app.
//app.UseHttpsRedirection();

// Add routing to the app.
app.UseRouting();

// Add the SPA statis files to the app.
app.UseSpaStaticFiles();

// Bind the CORS header to our app.
app.UseCors("CorsPolicy");

// Disabled feature, it may be re-enabled in the future.
//app.UseAuthorization();

// Add MVC to the app.
app.UseMvc();

// Configure the SPA route if it's production or development.
app.UseSpa(spa =>
{
    if (app.Environment.IsDevelopment())
    {
        spa.Options.SourcePath = $"{SPA_ROOT_PATH}/";
        spa.Options.StartupTimeout = TimeSpan.FromSeconds(120);
        spa.UseVueCli(npmScript: "dev");
    }
    else
    {
        spa.Options.SourcePath = SPA_ROOT_PATH;
    }
});

// Add controllers to the app
app.MapControllers();

// Initialize the app
app.Run();