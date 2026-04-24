namespace ApplicationTracker.Web.Services.Authorization
{
	public interface ITokenProvider
	{
		Task<string> GetTokenAsync();
	}
}
