using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace ApplicationTracker.Web.Services.Authentication
{
	public class AuthenticationService(AuthenticationStateProvider _authenticationStateProvider) : IAuthenticationService
	{
		public async Task<bool> IsUserAuthenticatedAsync()
		{
			var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
			return authState.User.Identity?.IsAuthenticated ?? false;
		}

		public async Task<string> GetUserNameAsync()
		{
			var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
			return authState.User.Identity?.IsAuthenticated == true
				? authState.User.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? "Unknown"
				: "Guest";
		}

		public async Task<ClaimsPrincipal> GetUserAsync()
		{
			var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
			return authState.User;
		}

		public async Task<Guid?> GetUserIdAsync()
		{
			var user = await GetUserAsync();
			if (user.Identity?.IsAuthenticated != true)
				return null;

			// Common claim types that contain the user's object id
			string[] claimTypes = new[] { "oid", ClaimTypes.NameIdentifier, "sub" };

			foreach (var ct in claimTypes)
			{
				var claim = user.Claims.FirstOrDefault(c => string.Equals(c.Type, ct, StringComparison.OrdinalIgnoreCase));
				if (claim != null && Guid.TryParse(claim.Value, out var parsed))
					return parsed;
			}

			// As a last resort, find any claim value that is a GUID
			var guidValue = user.Claims.Select(c => c.Value).FirstOrDefault(v => Guid.TryParse(v, out _));
			if (!string.IsNullOrEmpty(guidValue) && Guid.TryParse(guidValue, out var fallback))
				return fallback;

			return null;
		}
	}
}
