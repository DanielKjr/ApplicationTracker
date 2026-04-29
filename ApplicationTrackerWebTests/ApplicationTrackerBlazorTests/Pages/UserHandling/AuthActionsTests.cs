using System.Collections.Generic;
using System.Security.Claims;
using AngleSharp.Dom;
using ApplicationTracker.ApiClient;
using ApplicationTracker.Web.Components.Pages.UserHandling;
using ApplicationTracker.Web.Components.Shared;
using ApplicationTracker.Web.Services.Authentication;
using ApplicationTrackerBlazorTests.Setups;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Moq;

namespace ApplicationTrackerBlazorTests.Pages.UserHandling
{
	[TestFixture]
	[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
	class AuthActionsTests : BunitContext
	{


		[Test]
		public void AuthActions_Unauthorized_Displays_Login()
		{

			Services.RegisterApplicationTrackerApi();
			var auth = AddAuthorization();
			Services.AddTransient<IJSModuleLoader, JSModuleLoader>();
			JSInterop.SetupModule("./js/clipboardModule.js");


			var cut = Render<CascadingAuthenticationState>(parameters => parameters
				.AddChildContent<AuthActions>()
			);

			cut.Find("div.name").MarkupMatches(@"<div class=""name"">Guest</div>");
			var loginButton = cut.Find("button.b1");
			Assert.AreEqual("Login", loginButton.TextContent);
		}
		[Test]
		public void AuthActions_Authorized_Displays_Logout()
		{
			this.SetAuthorized();
			Services.RegisterApplicationTrackerApi();
			Services.AddTransient<IJSModuleLoader, JSModuleLoader>();
			JSInterop.SetupModule("./js/clipboardModule.js");

			var cut = Render<CascadingAuthenticationState>(parameters => parameters
				.AddChildContent<AuthActions>()
			);


			var username = cut.Find("div.name");
			var logoutButton = cut.Find("button.b1");
			Assert.AreEqual("Logout", logoutButton.TextContent);
			Assert.AreEqual("NameOfUser", username.TextContent);
		}

		[Test]
		public void AuthActions_Displays_Authorizing()
		{
			Services.RegisterApplicationTrackerApi();
			Services.AddTransient<IJSModuleLoader, JSModuleLoader>();
			JSInterop.SetupModule("./js/clipboardModule.js");
			AddAuthorization().SetAuthorizing();
			var cut = Render<CascadingAuthenticationState>(parameters => parameters
				.AddChildContent<AuthActions>()
			);
			var userAction = cut.Find("div.name");

			Assert.AreEqual("Authorizing...", userAction.TextContent);

		}

		[Test]
		public void AuthActions_Login_Redirects_To_Microsoft()
		{
			this.SetAuthorized();
			Services.RegisterApplicationTrackerApi();
			Services.AddTransient<IJSModuleLoader, JSModuleLoader>();
			JSInterop.SetupModule("./js/clipboardModule.js");
			var cut = Render<CascadingAuthenticationState>(parameters => parameters
				.AddChildContent<AuthActions>()
			);

			NavigationManager? navigationManager = Services.GetRequiredService<NavigationManager>() as NavigationManager;
			IElement logoutButton = cut.Find("button.b1");
			Assert.AreEqual("Logout", logoutButton.TextContent);
			logoutButton.Click();
			string url = navigationManager!.Uri;
			Assert.AreEqual("http://localhost/Account/LogOut", url);

		}

		


		[Test]
		public void AuthActions_Logout_Redirects_To_Microsoft()
		{
			Services.RegisterApplicationTrackerApi();
			var auth = AddAuthorization();
			Services.AddTransient<IJSModuleLoader, JSModuleLoader>();
			JSInterop.SetupModule("./js/clipboardModule.js");
			IRenderedComponent<CascadingAuthenticationState>? cut = Render<CascadingAuthenticationState>(parameters => parameters
				.AddChildContent<AuthActions>()
			);

			NavigationManager? navigationManager = Services.GetRequiredService<NavigationManager>() as NavigationManager;
			IElement loginButton = cut.Find("button.b1");
			Assert.AreEqual("Login", loginButton.TextContent);
			loginButton.Click();
			string url = navigationManager!.Uri;
			Assert.AreEqual("http://localhost/Account/Login", url);


		}


		[Test]
		public void AuthActions_UserName_Click_Copies_UserID()
		{
			//TODO need to simplify this. This is a niche case where i need to test
			//the fetched token and have to verify it, but its not pretty
			this.SetAuthorized();
			var expectedGuid = Guid.NewGuid().ToString();
			var tokenAcquisitionMock = new Mock<ITokenAcquisition>();
			tokenAcquisitionMock.Setup(t => t.GetAccessTokenForUserAsync(
				It.IsAny<IEnumerable<string>>(), It.IsAny<string>(),
				It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(),
				It.IsAny<TokenAcquisitionOptions>())).ReturnsAsync(expectedGuid);

			var httpClientFactoryMock = new Mock<IHttpClientFactory>();
			var configurationMock = new Mock<IConfiguration>();
			var apiClientMock = new Mock<ApplicationTrackerApiClient>("http://dummy", new HttpClient());
			var applicationTrackerApi = new TokenFetcherService(
				tokenAcquisitionMock.Object,
				httpClientFactoryMock.Object,
				configurationMock.Object,
				apiClientMock.Object
			);
			Services.AddSingleton<TokenFetcherService>(applicationTrackerApi);
			Services.AddSingleton<IAuthenticationService, AuthenticationService>();
			Services.AddTransient<IJSModuleLoader, JSModuleLoader>();
			var clipboard = JSInterop.SetupModule("./js/clipboardModule.js");
			clipboard.Setup<bool>("copyTextToClipboard", args => true)
				.SetResult(true);

			var cut = Render<CascadingAuthenticationState>(parameters => parameters
				.AddChildContent<AuthActions>()
			);

			cut.Find("div.name").Click();

			cut.WaitForAssertion(() =>
			{
				var invocation = clipboard.Invocations
					.Single(i => i.Identifier == "copyTextToClipboard");

				Assert.AreEqual(expectedGuid, invocation.Arguments[0]);
			});

		}

	}
}
