using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationTracker.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[Tags("Debug tool to check token content")]
	public class JwtReader : Controller
	{
		[HttpPost("/jwt")]
		public async Task<IActionResult> Decode(string jwt)
		{
			JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
			return  Ok(handler.ReadJwtToken(jwt));
		}
	}
}
