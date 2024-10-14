using Library_Management_System.Exceptions;
using Library_Management_System.Models;
using Library_Management_System.Services.v1.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class CheckoutAndReturnController : ControllerBase
	{
		private readonly ICheckoutAndReturnsService _checkoutAndReturnsService;
		public CheckoutAndReturnController(ICheckoutAndReturnsService checkoutAndReturnsService)
		{
			_checkoutAndReturnsService = checkoutAndReturnsService;
		}

		[HttpGet("checkOverdueBooks")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> CheckOverdueBooksAsync(DateTime currentDate)
		{
			var overdueBooks = await _checkoutAndReturnsService.checkOverdueBooksAsync(currentDate);
			if (overdueBooks == null || !overdueBooks.Any())
			{
				return Ok("Geri qaytarılmayan kitab yoxdur.");
			}

			return Ok(overdueBooks);
		}

		[HttpPost("sendOverdueNotices")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> SendOverdueNoticesAsync([FromBody] List<Rental> overdueBooks)
		{
			var result = await _checkoutAndReturnsService.sendOverdueNoticesAsync();
			if (!result)
			{
				return BadRequest("Failed to send overdue notices.");
			}
			return Ok("Notices sent successfully.");
		}

		[HttpPost("logReturnEvent")]
		public async Task<IActionResult> LogReturnEvent(int bookId, int userId, DateTime returnDate)
		{
			try
			{
				await _checkoutAndReturnsService.logReturnEventAsync(bookId, userId, returnDate);
				
				return Ok("Return event logged successfully.");
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message : {ex.Message}");
			}
		}

		[HttpPut("updateBookStatus/{rentalId}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> UpdateBookStatusAsync(int rentalId)
		{
			try
			{
				var success = await _checkoutAndReturnsService.updateBookStatusAsync(rentalId);
				if (success)
				{
					return Ok("Book status updated successfully.");
				}
				else
				{
					throw new LibraryManagementSystemException("Kirayə yoxdur, ya da qeyriaktivdir");
				}

			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message : {ex.Message}");
			}

		}

	}
}
