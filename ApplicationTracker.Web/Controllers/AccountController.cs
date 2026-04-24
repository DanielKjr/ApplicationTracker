namespace ApplicationTracker.Web.Controllers
{
	using Microsoft.AspNetCore.Authentication;
	using Microsoft.AspNetCore.Authentication.Cookies;
	using Microsoft.AspNetCore.Authentication.OpenIdConnect;
	using Microsoft.AspNetCore.Mvc;

	[Route("[controller]/[action]")]
	public class AccountController : Controller
	{
		[HttpGet]
		public IActionResult Login(string redirectUri = "/")
		{
			return Challenge(
				new AuthenticationProperties { RedirectUri = redirectUri },
				OpenIdConnectDefaults.AuthenticationScheme
			);
		}

		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
			return Redirect("/");
		}
	}

}
