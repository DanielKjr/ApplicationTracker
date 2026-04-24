
using System.Security.Claims;

namespace ApplicationTracker.Api.Utility
{
	public class UserHelper() : IUserHelper
	{
		//TODO for the class to make sense it needs more utility, i.e. get name or what not
		public Guid GetUserId(ClaimsPrincipal claim)
		{
			return (Guid.Parse(claim.FindFirstValue("http://schemas.microsoft.com/identity/claims/objectidentifier")!));
		}
	}
}
