using Library_Management_System.DTOs.Reservation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Library_Management_System.Exceptions;
using Library_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Library_Management_System.Services.v1.Interfaces;

namespace Library_Management_System.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class ReservationController : ControllerBase
	{
		private readonly IReservationService _reservationService;
		public ReservationController(IReservationService reservationService)
		{
			_reservationService = reservationService;
		}

		[HttpGet("getReservationDetails/{reservationId}")]
		[Authorize(Roles = "Superadmin,Admin")]
		public async Task<IActionResult> GetReservationDetailsAsync(int reservationId)
		{
			try
			{
				var reservation = await _reservationService.getReservationDetailsAsync(reservationId);

				return Ok(reservation);
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}

		}

		[HttpGet("getBookReservations")]
		[Authorize(Roles = "Superadmin,Admin")]
		public async Task<IActionResult> GetBookReservations([FromQuery] int bookId)
		{
			try
			{
				var reservations = await _reservationService.getBookReservationsAsync(bookId);
				if (!reservations.Any())
				{
					throw new LibraryManagementSystemException("Rezervasiya yoxdur.");
				}

				return Ok(reservations);
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}

		}

		[HttpGet("getUserReservations")]
		[Authorize(Roles = "Superadmin,Admin")]
		public async Task<IActionResult> GetUserReservationsAsync([FromQuery] int userId)
		{
			try
			{
				var reservations = await _reservationService.getUserReservationsAsync(userId);
				if (!reservations.Any())
				{
					throw new LibraryManagementSystemException("Rezervasiya tapılmadı.");
				}

				return Ok(reservations);
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}

		}

		[HttpGet("checkAvailability")]
		[Authorize(Roles = "Superadmin,Admin")]
		public async Task<IActionResult> CheckAvailability(int bookId, DateTime startDate, DateTime endDate)
		{
			try
			{
				var isAvailable = await _reservationService.checkAvailabilityAsync(bookId, startDate, endDate);

				if (isAvailable)
				{
					return Ok("Kitab mövcuddur.");
				}
				else
				{
					return Ok("Kitab seçilən tarixlərdə mövcud deyil.");
				}
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}

		}

		[HttpPost("addReservation")]
		public async Task<IActionResult> AddReservationAsync([FromBody] AddReservationDTO reservation)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var newReservation = await _reservationService.addReservationAsync(reservation);
				return Ok(newReservation);
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}
		}

		[HttpPut("cancelReservation")]
		[Authorize(Roles = "Superadmin,Admin")]
		public async Task<IActionResult> CancelReservationAsync( [FromQuery] int reservationId)
		{
			try
			{
				var success = await _reservationService.cancelReservationAsync(reservationId);
				if (!success)
				{
					throw new LibraryManagementSystemException("Reservasiya yoxdur, ya da reservasiya artıq ləğv edilib");
				}
				else
				{
					return Ok("Reservasiya ləğv edildi.");
				}
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}

		}

		[HttpPut("updateReservation/{reservationId}")]
		[Authorize(Roles = "Superadmin,Admin")]
		public async Task<IActionResult> UpdateReservationAsync(int reservationId , [FromBody] UpdateReservationDTO newReservation)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var reservation = await _reservationService.updateReservationAsync(reservationId, newReservation);
				if (!reservation)
				{
					throw new LibraryManagementSystemException("Reservasiya tapılmadı.");
				}
				else
				{
					return Ok("Rezervasiya məlumatları yeniləndi.");
				}

			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}
		}

	}
}
