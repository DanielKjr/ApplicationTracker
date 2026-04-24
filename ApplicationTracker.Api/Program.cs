using ApplicationTracker.Api.Services;
using ApplicationTracker.Api.Services.Interfaces;
using ApplicationTracker.Api.Utility;
using ApplicationTracker.Api.Utility.ServiceExtentions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();



builder.Services.RegisterRepository();

builder.Services.AddScoped<IApplicationService, ApplicationService>();

builder.Services.AddTransient<IUserHelper, UserHelper>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<HttpContextEnricher>();

builder.Services.ConfigureSwagger();
builder.Configuration.AddJsonSecrets();
builder.Host.AddSerilogEnricher();


//TODO cors configuration not confirmed to be functioning right yet.
var frontEnd = "https://localhost:44394";
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowFrontend", policy =>
	{
		policy.WithOrigins(frontEnd)
			  .AllowAnyMethod()
			  .WithHeaders("Authorization", "Content-Type")
			  .AllowCredentials();
	});
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("Entra:ApplicationTrackerApi"));

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.UseSwagger();
	app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
