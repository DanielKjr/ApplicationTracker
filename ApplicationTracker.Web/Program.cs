using ApplicationTracker.Web.Components;
using ApplicationTracker.Web.Components.Shared;
using ApplicationTracker.Web.Services.Authentication;
using ApplicationTracker.Web.Utilities.ServiceExtentions;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents(options =>
	options.DetailedErrors = builder.Environment.IsDevelopment())
	.AddInteractiveServerComponents();
builder.Services.AddControllersWithViews();
//StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);



builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddMicrosoftGraphClient("https://graph.microsoft.com/User.Read");
builder.Services.AddMsalAuthentication(options =>
{
	builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
	options.ProviderOptions.DefaultAccessTokenScopes.Add("https://graph.microsoft.com/User.Read");
});
builder.Services.AddHttpClient();

builder.Services.AddTransient<IJSModuleLoader, JSModuleLoader>();

builder.Host.AddSerilogEnricher();
builder.Services.AddLoggingConfiguration(builder.Configuration);
builder.Services.AddApiConfiguration(builder.Configuration);
builder.Services.RegisterTokenCache(builder.Configuration);
builder.Services.RegisterAuthenticationAndAuthorization(builder.Configuration);
var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	app.UseHsts();
}
else
{
	app.UseDeveloperExceptionPage();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapControllers();
app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.Run();
