using System.Security.Claims;
using ApplicationTracker.Api.Model.Dto;
using ApplicationTracker.Api.Services.Interfaces;
using ApplicationTracker.Api.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ApplicationTracker.Api.Controllers
{

	[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class ApplicationController(IApplicationService _applicationService, IUserHelper userHelper) : ControllerBase
	{

		[HttpPost("new")]
		public async Task<IActionResult> AddNew([FromBody] NewJobApplicationDto dto)
		{
			var userId = userHelper.GetUserId(User);
			try
			{
				await _applicationService.AddNewAsync(userId,dto);
				return Ok();
			}
			catch (ArgumentNullException ex)
			{
				return BadRequest(new { error = ex.Message });
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { error = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
			}
		}

		[HttpPost("new/multiple")]
		public async Task<IActionResult> AddMultipleNew([FromBody] IEnumerable<NewJobApplicationDto> dtos)
		{
			var userId = userHelper.GetUserId(User);
			try
			{
				await _applicationService.AddNewAsync(userId, dtos);
				return Ok();
			}
			catch (ArgumentNullException ex)
			{
				return BadRequest(new { error = ex.Message });
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { error = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
			}
		}

		[HttpGet("userApplications")]
		[ProducesResponseType(typeof(IEnumerable<JobApplicationDisplayDto>), 200)]
		public async Task<IActionResult> GetAllForUser()
		{
			var userID = userHelper.GetUserId(User);
		
			try
			{
				var items = await _applicationService.GetAllAsync<JobApplicationDisplayDto>(userID);
				if (items == null || !items.Any())
					return NotFound(new { error = "No applications found for this user." });

				return Ok(items);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new { error = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
			}
		}

		[HttpGet("application")]
		[ProducesResponseType(typeof(JobApplicationDisplayDto), 200)]
		public async Task<IActionResult> GetApplicationById(Guid applicationId)
		{
			var userId = userHelper.GetUserId(User);
			try
			{
				var application = await _applicationService.GetDisplayDtoById(userId, applicationId);
				if (application == null) return NotFound();
				return Ok(application);
			}
			catch(ArgumentException ex)
			{
				return BadRequest(new { error = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
			}
		}

		[HttpDelete("delete")]
		public async Task<IActionResult> Delete(Guid applicationId)
		{
			var userId = userHelper.GetUserId(User);
			try
			{
				await _applicationService.DeleteByIdAsync(userId, applicationId);
				return Ok();
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { error = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
			}
		}

		[HttpDelete("delete/multiple")]
		public async Task<IActionResult> DeleteMultiple([FromBody] IEnumerable<Guid> applicationIds)
		{
			var userId = userHelper.GetUserId(User);
			try
			{
				await _applicationService.DeleteByIdAsync(userId, applicationIds);
				return Ok();
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { error = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
			}
		}

		[HttpPatch("update")]
		public async Task<IActionResult> Update([FromBody] JobApplicationDisplayDto dto)
		{
			var userId = userHelper.GetUserId(User);
			try
			{
				await _applicationService.UpdateApplicationAsync(userId, dto);
				return Ok();
			}
			catch (ArgumentNullException ex)
			{
				return BadRequest(new { error = ex.Message });
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { error = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
			}
		}

		[HttpPatch("update/multiple")]
		public async Task<IActionResult> UpdateMultiple([FromBody] IEnumerable<JobApplicationDisplayDto> dtos)
		{
			var userId = userHelper.GetUserId(User);
			try
			{
				await _applicationService.UpdateApplicationAsync(userId, dtos);
				return Ok();
			}
			catch (ArgumentNullException ex)
			{
				return BadRequest(new { error = ex.Message });
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { error = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
			}
		}



		[HttpGet("temp")]
		[Produces("text/plain")]
		[ProducesResponseType(typeof(string), StatusCodes.Status200OK, "text/plain")]
		public string Temp()
		{
			Log.Warning("API out here");
			return "accessed";
		}
	}
}
