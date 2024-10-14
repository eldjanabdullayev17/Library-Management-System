using Library_Management_System.DTOs.Rental;
using Library_Management_System.Exceptions;
using Library_Management_System.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class RentalController : ControllerBase
	{
		private readonly IRentalService _rentalService;
        public RentalController(IRentalService rentalService)
        {
			_rentalService = rentalService;
        }


        [HttpGet("getRentalById/{rentalId}")]
		public async Task<IActionResult> GetRentalByIdAsync(int rentalId)
		{
			try
			{
				var rental = await _rentalService.getRentalByIdAsync(rentalId);
				return Ok(rental);
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message : {ex.Message}");
			}
		}

		[HttpPost("addRental")]
		public async Task<IActionResult> AddRentalAsync(AddRentalDTO rental)
		{
			try
			{
				var newRental = await _rentalService.addRentalAsync(rental);
				return Ok(newRental);
			}
			catch(LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message : {ex.Message}");
			}
		}

		[HttpPut("updateRental/{rentalId}")]
		public async Task<IActionResult> UpdateRentalAsync(int rentalId,UpdateRentalDTO newRental)
		{
			try
			{
				var rental = await _rentalService.updateRentalAsync(rentalId, newRental);
				if (!rental)
				{
					throw new LibraryManagementSystemException("Kirayə yoxdur");
				}
				else
				{
					return Ok(rental);
				}
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message : {ex.Message}");
			}
		}
	}


}
