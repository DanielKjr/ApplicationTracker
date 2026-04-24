
using Microsoft.Identity.Web;

namespace ApplicationTracker.Web.Services.Authorization
{
	public class TokenProvider : ITokenProvider
	{
		private readonly ITokenAcquisition _tokenAcquisition;
		private readonly IConfiguration _configuration;

		public TokenProvider(ITokenAcquisition tokenAcquisition, IConfiguration configuration)
		{
			_tokenAcquisition = tokenAcquisition;
			_configuration = configuration;
		}

		public async Task<string> GetTokenAsync()
		{
			var scopes = _configuration["Entra:Blazor:Scopes"]?.Split(' ') ?? Array.Empty<string>();
			return await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);
		}
	}
}
