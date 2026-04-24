using System.Security.Claims;

namespace ApplicationTracker.Web.Services.Authentication
{
	public interface IAuthenticationService
	{
		Task<ClaimsPrincipal> GetUserAsync();
		Task<Guid?> GetUserIdAsync();
		Task<string> GetUserNameAsync();
		Task<bool> IsUserAuthenticatedAsync();
	}
}