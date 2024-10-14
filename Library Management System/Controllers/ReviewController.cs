using Library_Management_System.Models;
using Library_Management_System.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Library_Management_System.DTOs.Review;
using Library_Management_System.Services.v1.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Library_Management_System.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class ReviewController : ControllerBase
	{
		private readonly IReviewService _reviewService;
		public ReviewController(IReviewService reviewService)
		{
			_reviewService = reviewService;
		}

		[HttpPost("addReview")]
		public async Task<IActionResult> AddReviewAsync([FromBody] AddReviewDTO review)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var newReview = await _reviewService.addReviewAsync(review);
				return Ok(newReview);
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message : {ex.Message}");
			}

		}

		[HttpPut("updateReview/{reviewId}")]
		public async Task<IActionResult> UpdateReviewAsync(int reviewId, [FromBody] UpdateReviewDTO newReview)
		{
			try
			{
				var success = await _reviewService.updateReviewAsync(reviewId, newReview);
				if (!success)
				{
					throw new LibraryManagementSystemException("Rəy tapılmadı.");
				}
				else
				{
					return Ok("Rəy yeniləndi");
				}
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message : {ex.Message}");
			}
		}

		[HttpGet("getAllReviewsForBook")]
		[Authorize(Roles = "Superadmin")]
		public async Task<IActionResult> GetAllReviewsForBookAsync([FromQuery] int bookId)
		{
			try
			{
				var allReviewsForBook = await _reviewService.getAllReviewsForBookAsync(bookId);
				return Ok(allReviewsForBook);

			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message : {ex.Message}");
			}
		}

		[HttpGet("getUserReview")]
		[Authorize(Roles = "Superadmin")]
		public async Task<IActionResult> GetUserReviewAsync([FromQuery] int userId, [FromQuery] int bookId)
		{
			try
			{
				var userReview = await _reviewService.getUserReviewAsync(userId, bookId);
				return Ok(userReview);
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message : {ex.Message}");
			}
		}

		[HttpDelete("deleteReview/{reviewId}")]
		[Authorize(Roles = "Superadmin")]
		public async Task<IActionResult> DeleteReviewAsync(int reviewId)
		{
			try
			{
				var success = await _reviewService.deleteReviewAsync(reviewId);
				if (!success)
				{
					throw new LibraryManagementSystemException("Rəy tapılmadı.");
				}
				else
				{
					return Ok("Rəy silindi.");
				}
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message : {ex.Message}");
			}
		}

	}
}
