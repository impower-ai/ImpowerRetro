using ImpowerRetro.Components;
using ImpowerRetro.Components.Utilities;
using ImpowerRetro.Services;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Configure logging with console output (Railway captures these)
builder.Logging.AddConsole();
builder.Logging.AddFilter("Microsoft.AspNetCore", LogLevel.Warning);
builder.Logging.AddFilter("Microsoft.Extensions.Hosting", LogLevel.Warning);

// Create a logger for configuration checking and startup process
var loggerFactory = LoggerFactory.Create(configure => configure.AddConsole());
var configLogger = loggerFactory.CreateLogger<Program>();

configLogger.LogInformation("⏳ STARTUP: Initializing ImpowerRetro application in {Environment} environment", builder.Environment.EnvironmentName);

configLogger.LogInformation("⏳ SERVICES: Registering ASP.NET Core infrastructure components");
builder.Services.AddRazorComponents()
	   .AddInteractiveServerComponents()
	   .AddHubOptions(options => options.MaximumReceiveMessageSize = 10 * 1024 * 1024);

builder.Services.AddControllers();
builder.Services.AddRadzenComponents();
configLogger.LogInformation("✅ SERVICES: ASP.NET Core infrastructure components registered");

configLogger.LogInformation("⏳ APP_SERVICES: Registering core service dependencies");
// Register LogService first since it's a dependency for other services
builder.Services.AddSingleton<LogService>();
builder.Services.AddSingleton<ILogService>(sp => sp.GetRequiredService<LogService>());

// Register other services with their interfaces
builder.Services.AddSingleton<SessionService>();
builder.Services.AddSingleton<ISessionService>(sp => sp.GetRequiredService<SessionService>());
builder.Services.AddScoped<MemoryService>();
builder.Services.AddScoped<IMemoryService>(sp => sp.GetRequiredService<MemoryService>());
builder.Services.AddScoped<IJSUtilityService, JSUtilityService>();

configLogger.LogInformation("⏳ APP_SERVICES: Configuring Radzen UI components");
builder.Services.AddRadzenCookieThemeService(options =>
{
	options.Name = Constants.App.ThemeCookieName;
	options.Duration = TimeSpan.FromDays(365);
});

builder.Services.AddHttpClient();
configLogger.LogInformation("✅ APP_SERVICES: Application service registration completed");

configLogger.LogInformation("⏳ BUILD: Building application host instance");
var app = builder.Build();
configLogger.LogInformation("✅ BUILD: Application host instance successfully constructed");

// Configure the HTTP request pipeline
configLogger.LogInformation("⏳ HTTP_PIPELINE: Configuring ASP.NET Core request pipeline");
if (!app.Environment.IsDevelopment())
{
	configLogger.LogInformation("⏳ HTTP_PIPELINE: Configuring production security middleware (HSTS, Error handling)");
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

configLogger.LogInformation("⏳ HTTP_PIPELINE: Configuring HTTPS redirection middleware");
app.UseHttpsRedirection();

configLogger.LogInformation("⏳ HTTP_PIPELINE: Registering API controllers");
app.MapControllers();

configLogger.LogInformation("⏳ HTTP_PIPELINE: Enabling static file middleware");
app.UseStaticFiles();

configLogger.LogInformation("⏳ HTTP_PIPELINE: Configuring antiforgery protection");
app.UseAntiforgery();

configLogger.LogInformation("⏳ HTTP_PIPELINE: Mapping Razor component endpoints");
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

configLogger.LogInformation("✅ HTTP_PIPELINE: Request pipeline configuration completed");

configLogger.LogInformation("✅ APPLICATION: ImpowerRetro startup sequence completed successfully");

app.Run();
