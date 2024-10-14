using Library_Management_System.Exceptions;
using Library_Management_System.Services.v1.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class ReportController : ControllerBase
	{
		private readonly IReportService _reportService;
		public ReportController(IReportService reportService)
		{
			_reportService = reportService;
		}

		[HttpGet("getMostReadBooks")]
		[Authorize(Roles = "Superadmin,Admin")]
		public async Task<IActionResult> GetMostReadBooks()
		{
			var books = await _reportService.getMostReadBooksAsync();
			return Ok(books);
		}

		[HttpGet("getBookRentalHistory")]
		[Authorize(Roles = "Superadmin,Admin")]
		public async Task<IActionResult> GetBookRentalHistory([FromQuery] int bookId)
		{
			try
			{
				var history = await _reportService.getBookRentalHistoryAsync(bookId);
				return Ok(history);
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message : {ex.Message}");
			}

		}

		[HttpGet("generateRentalStatistics")]
		[Authorize(Roles = "Superadmin,Admin")]
		public async Task<IActionResult> GenerateRentalStatisticsAsync()
		{
			var rentalStatistics = await _reportService.generateRentalStatisticsAsync();
			return Ok(rentalStatistics);
		}

		[HttpGet("generateUserActivityReport")]
		//[Authorize(Roles = "Superadmin,Admin")]
		public async Task<IActionResult> GenerateUserActivityReportAsync( [FromQuery] int userId)
		{
			var userActivity = await _reportService.generateUserActivityReportAsync(userId);
			return Ok(userActivity);
		}

		[HttpGet("getUserLoginHistory")]
		//[Authorize(Roles = "Superadmin,Admin")]
		public async Task<IActionResult> GetUserLoginHistoryAsync([FromQuery] int userId)
		{
			try
			{
				var userLoginHistory = await _reportService.getUserLoginHistoryAsync(userId);
				return Ok(userLoginHistory);	
			}
			catch(LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message : {ex.Message}");
			}
		}
	}
}
