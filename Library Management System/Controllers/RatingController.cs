using Library_Management_System.DTOs.Rating;
using Library_Management_System.Exceptions;
using Library_Management_System.Services.v1.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class RatingController : ControllerBase
	{
		private readonly IRatingService _ratingService;
		public RatingController(IRatingService ratingService)
		{
			_ratingService = ratingService;
		}

		[HttpPost("addRating")]
		public async Task<IActionResult> AddRatingAsync([FromBody] AddRatingDTO rating)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			try
			{
				var newRating = await _ratingService.addRatingAsync(rating);
				return Ok(newRating);
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message : {ex.Message}");
			}
		}

		[HttpPut("updateRating/{ratingId}")]
		public async Task<IActionResult> UpdateRatingAsync(int ratingId, [FromBody] UpdateRatingDTO newRating)
		{
			try
			{
				var success = await _ratingService.updateRatingAsync(ratingId, newRating);
				if (!success)
				{
					throw new LibraryManagementSystemException("Reytinq tapılmadı.");
				}
				else
				{
					return Ok("Reytinq dəyişdirildi");
				}
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message : {ex.Message}");
			}
		}

		[HttpGet("getUserRating")]
		[Authorize(Roles = "Superadmin")]
		public async Task<IActionResult> GetUserRatingAsync([FromQuery] int userId, [FromQuery] int bookId)
		{
			try
			{
				var rating = await _ratingService.getUserRatingAsync(userId, bookId);
				return Ok(rating);
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message : {ex.Message}");
			}
		}

		[HttpGet("getAverageRating")]
		[Authorize(Roles = "Superadmin")]
		public async Task<IActionResult> GetAverageRating([FromQuery] int bookId)
		{
			try
			{
				var averageRating = await _ratingService.getAverageRatingAsync(bookId);
				return Ok(averageRating);
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message : {ex.Message}");
			}

		}

		[HttpDelete("deleteRating/{ratingId}")]
		[Authorize(Roles = "Superadmin")]
		public async Task<IActionResult> DeleteRatingAsync(int ratingId)
		{
			try
			{
				var success = await _ratingService.deleteRatingAsync(ratingId);

				if (!success)
				{
					throw new LibraryManagementSystemException("Reytinq tapılmadı.");
				}
				else
				{
					return Ok("Reytinq silindi.");
				}

			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message : {ex.Message}");
			}
		}

		[HttpGet("getRatingsAndReviewsByBook/{bookId}")]
		[Authorize(Roles = "Superadmin")]
		public async Task<IActionResult> GetRatingsAndReviewsByBookAsync(int bookId)
		{
			try
			{
				var ratingAndReview = await _ratingService.getRatingsAndReviewsByBookAsync(bookId);
				return Ok(ratingAndReview);
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message : {ex.Message}");
			}
		}

		[HttpGet("getRatingsAndReviewsByUser/{userId}")]
		[Authorize(Roles = "Superadmin")]
		public async Task<IActionResult> GetRatingsAndReviewsByUserAsync(int userId)
		{
			try
			{
				var ratingAndReview = await _ratingService.getRatingsAndReviewsByUserAsync(userId);
				return Ok(ratingAndReview);
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message : {ex.Message}");
			}
		}

		[HttpPost("addRatingAndReview")]
		public async Task<IActionResult> AddRatingAndReviewAsync([FromBody] AddRatingAndReviewDTO ratingAndReview)
		{
			try
			{
				var newRatingAndReview = await _ratingService.addRatingAndReviewAsync(ratingAndReview);
				return Ok(newRatingAndReview);
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message : {ex.Message}");
			}
		}

		[HttpPut("updateRatingAndReview")]
		public async Task<IActionResult> UpdateRatingAndReviewAsync([FromQuery] int bookId, [FromQuery] int userId, [FromBody] UpdateRatingAndReviewDTO newUpdateRatingAndReview)
		{
			try
			{
				var result = await _ratingService.updateRatingAndReviewAsync(bookId, userId, newUpdateRatingAndReview);

				if (!result)
				{
					throw new LibraryManagementSystemException("Reytinq və ya rəy tapılmadı.");
				}

				return Ok("Reytinq və rəy uğurla yeniləndi.");
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message : {ex.Message}");
			}
		}

		[HttpDelete("deleteRatingAndReview")]
		[Authorize(Roles = "Superadmin")]
		public async Task<IActionResult> DeleteRatingAndReviewAsync([FromQuery] int bookId, [FromQuery] int userId)
		{
			try
			{
				var result = await _ratingService.deleteRatingAndReviewAsync(bookId, userId);

				if (!result)
				{
					throw new LibraryManagementSystemException("Reytinq və ya rəy tapılmadı.");
				}

				return Ok("Reytinq və rəy uğurla silindi.");
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message : {ex.Message}");
			}
		}
	}
}
