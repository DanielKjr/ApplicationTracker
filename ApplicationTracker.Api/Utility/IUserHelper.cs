using System.Security.Claims;

namespace ApplicationTracker.Api.Utility
{
	public interface IUserHelper
	{
		public Guid GetUserId(ClaimsPrincipal claim);
	}
}
