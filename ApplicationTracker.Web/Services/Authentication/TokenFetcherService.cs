using ApplicationTracker.ApiClient;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;

namespace ApplicationTracker.Web.Services.Authentication
{
	public class TokenFetcherService(ITokenAcquisition tokentAcquisition, IHttpClientFactory httpClientFactory,
		IConfiguration configuration, ApplicationTrackerApiClient applicationTrackerApi)
	{

		//debug method used to test bearer token earlier
		public async Task<HttpResponseMessage> CallApi()
		{
			var scopes = configuration["Entra:Blazor:Scopes"]?.Split(' ') ?? Array.Empty<string>();
			string accessToken = await tokentAcquisition.GetAccessTokenForUserAsync(scopes);
			var httpClient = httpClientFactory.CreateClient();
			httpClient.BaseAddress = new Uri(configuration["ApplicationTrackerApi:BaseUrl"]!);
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			return await httpClient.GetAsync("Application/temp");



		}
		public async Task<string> CallApiClient()
		{
			return await applicationTrackerApi.TempAsync();
		}

		public async Task<string> GetToken()
		{
			var scopes = configuration["Entra:Blazor:Scopes"]?.Split(' ') ?? Array.Empty<string>();
			return await tokentAcquisition.GetAccessTokenForUserAsync(scopes);
		}
	}
}
