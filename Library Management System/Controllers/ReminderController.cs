using Library_Management_System.DTOs.Reminder;
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
	public class ReminderController : ControllerBase
	{
		private readonly IReminderService _reminderService;
        public ReminderController(IReminderService reminderService)
        {
            _reminderService = reminderService;
        }

		[HttpPost("addReminder")]
		[Authorize(Roles = "Superadmin,Admin")]
		public async Task<IActionResult> AddReminderAsync([FromBody] AddReminderDTO reminder)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			try
			{
				var newReminder = await _reminderService.addReminderAsync(reminder);
				return Ok(newReminder);
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}
		}

		[HttpGet("sendReminderNotification")]
		//[Authorize(Roles = "Superadmin,Admin")]
		public async Task<IActionResult> SendReminderNotification(int userId, string message)
		{
			try
			{
				var success = await _reminderService.sendReminderNotificationAsync(userId, message);
				if (success)
				{
					return Ok("Xatırlatma bildirişi göndərildi.");
				}

				return NotFound("Istifadeci yoxdur");
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}
		}

		[HttpGet("getRemindersByUser/{userId}")]
		public async Task<IActionResult> GetRemindersByUserAsync(int userId)
		{
			try
			{
				var reminders = await _reminderService.getRemindersByUserAsync(userId);
				return Ok(reminders);
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}

		}

		[HttpGet("getUpcomingReminders")]
		public async Task<IActionResult> GetUpcomingRemindersAsync([FromQuery] DateTime date)
		{
			try
			{
				var reminders = await _reminderService.getUpcomingRemindersAsync(date);
				return Ok(reminders);
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}
			
		}

		[HttpDelete("removeReminder/{reminderId}")]
		[Authorize(Roles = "Superadmin")]
		public async Task<IActionResult> RemoveReminderAsync(int reminderId)
		{
			try
			{
				var success = await _reminderService.removeReminderAsync(reminderId);
				if (!success)
				{
					return NotFound("Xatırlatma yoxdur.");
				}

				return Ok("Xatırlatma silindi.");
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}


		}

	}
}
