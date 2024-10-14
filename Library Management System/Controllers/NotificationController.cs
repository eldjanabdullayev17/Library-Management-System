using Library_Management_System.DTOs.Notification;
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
	public class NotificationController : ControllerBase
	{
		private readonly INotificationService _notificationService;

		public NotificationController(INotificationService notificationService)
		{
			_notificationService = notificationService;
		}

		[HttpGet("getNotificationsByUser/{userId}")]
		[Authorize(Roles = "Superadmin")]
		public async Task<IActionResult> GetNotificationsByUserAsync(int userId)
		{
			try
			{
				var notifications = await _notificationService.getNotificationsByUserAsync(userId);
				return Ok(notifications);
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}
		}

		[HttpPost("addNewBookNotification")]
		[Authorize(Roles = "Superadmin,User")]
		public async Task<IActionResult> AddNewBookNotificationAsync([FromBody] AddNewBookNotificationDTO notification)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			try
			{
				var newBookNotification = await _notificationService.addNewBookNotificationAsync(notification);
				return Ok(newBookNotification);
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}
		}

		[HttpPost("addEventNotification")]
		[Authorize(Roles = "Superadmin,User")]
		public async Task<IActionResult> AddEventNotificationAsync([FromBody] AddEventNotificationDTO notification)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			try
			{
				var newEventNotification = await _notificationService.addEventNotificationAsync(notification);
				return Ok(newEventNotification);
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}
		}

		[HttpDelete("removeNotification/{notificationId}")]
		[Authorize(Roles = "Superadmin")]
		public async Task<IActionResult> RemoveNotification(int notificationId)
		{
			try
			{
				await _notificationService.removeNotificationAsync(notificationId);
				return Ok("Bildiriş silindi.");
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}
			
		}

	}
}
