using ApplicationTracker.Web.Services.Authorization;
using ApplicationTracker.Web.Services.Authentication;
using ApplicationTracker.ApiClient;

namespace ApplicationTracker.Web.Utilities.ServiceExtentions
{
	public static class ApiConfigExtention
	{
		public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			string baseUrl = configuration["ApplicationTrackerApi:BaseUrl"]!;
			services.AddTransient<TokenFetcherService>();
			services.AddScoped<ITokenProvider, TokenProvider>();
			services.AddTransient<BearerTokenHandler>();

			services.AddHttpClient<ApplicationTrackerApiClient>((sp, client) =>
			{
				client.BaseAddress = new Uri(baseUrl);
			})
			.AddHttpMessageHandler<BearerTokenHandler>();

			services.AddScoped(sp =>
			{
				var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
				var httpClient = httpClientFactory.CreateClient(nameof(ApplicationTrackerApiClient));
				return new ApplicationTrackerApiClient(baseUrl, httpClient);
			});
			return services;
		}
	}
}
