using System.Net.Http.Headers;

namespace ApplicationTracker.Web.Services.Authorization
{
	public class BearerTokenHandler : DelegatingHandler
	{
		private readonly ITokenProvider _tokenProvider;

		public BearerTokenHandler(ITokenProvider tokenProvider)
		{
			_tokenProvider = tokenProvider;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var token = await _tokenProvider.GetTokenAsync();
			if (!string.IsNullOrEmpty(token))
			{
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
			}
			return await base.SendAsync(request, cancellationToken);
		}
	}
}
