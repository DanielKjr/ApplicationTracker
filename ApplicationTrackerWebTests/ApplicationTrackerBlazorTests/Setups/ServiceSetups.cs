using System.Security.Claims;
using ApplicationTracker.ApiClient;
using ApplicationTracker.Web.Services.Authentication;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Moq;

namespace ApplicationTrackerBlazorTests.Setups
{

	public static class ServiceSetups
	{
		public static BunitAuthorizationContext SetAuthorized(this BunitContext context)
		{
			return context.AddAuthorization().SetAuthorized("NameOfUser").SetClaims(new Claim("name", "NameOfUser"), new Claim(ClaimTypes.Name, "Guest"));

		}
		public static IServiceCollection RegisterApplicationTrackerApi(this IServiceCollection services)
		{
			var tokenAcquisitionMock = new Mock<ITokenAcquisition>();
			var httpClientFactoryMock = new Mock<IHttpClientFactory>();
			var configurationMock = new Mock<IConfiguration>();
			var apiClientMock = new Mock<ApplicationTrackerApiClient>("http://dummy", new HttpClient());

			var applicationTrackerApi = new TokenFetcherService(
				tokenAcquisitionMock.Object,
				httpClientFactoryMock.Object,
				configurationMock.Object,
				apiClientMock.Object
			);

			services.AddSingleton<TokenFetcherService>(applicationTrackerApi);

			services.AddSingleton<IAuthenticationService, AuthenticationService>();
			return services;
		}


	}
}
