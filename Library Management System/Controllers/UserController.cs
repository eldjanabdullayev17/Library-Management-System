using Library_Management_System.DTOs.User;
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
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;
		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpPost("addUser")]
		[Authorize(Roles = "Superadmin")]
		public async Task<IActionResult> AddUserAsync([FromBody] AddUserDTO user)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var newUser = await _userService.addUserAsync(user);
				return Ok(newUser);
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}
		}

		[HttpPut("updateUser/{userId}")]
		[Authorize(Roles = "Superadmin,User")]
		public async Task<IActionResult> UpdateUserAsync(int userId, [FromBody] UpdateUserDTO newUser)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var user = await _userService.updateUserAsync(userId, newUser);

				if (!user)
				{
					throw new LibraryManagementSystemException("İstifadəçi tapılmadı.");
				}
				else
				{
					return Ok("İstidaçi məlumatları dəyişdirildi");
				}
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}

		}

		[HttpPut("deactivateUser")]
		[Authorize(Roles = "Superadmin")]
		public async Task<IActionResult> DeactiveUserAsync([FromQuery] int userId)
		{
			try
			{
				var user = await _userService.deactivateUserAsync(userId);

				if (!user)
				{
					throw new LibraryManagementSystemException("Yanlış id gondermisiniz, ya da istifadəçi hesabı deaktiv vəziyyətdədir.");
				}
				else
				{
					return Ok("İstifadəçi hesabı deaktiv edildi.");
				}
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}

		}

		[HttpGet("getAllUsers")]
		[Authorize(Roles = "Superadmin")]
		public async Task<IActionResult> GetAllUsersAsync()
		{
			var users = await _userService.getAllUsersAsync();
			return Ok(users);
		}

		[HttpGet("getUserById/{userId}")]
		[Authorize(Roles = "Superadmin")]
		public async Task<IActionResult> GetUserByIdAsync(int userId)
		{
			try
			{
				var user = await _userService.getUserByIdAsync(userId);
				return Ok(user);
				
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}
		}

		[HttpGet("getUsersByRole")]
		[Authorize(Roles = "Superadmin")]
		public async Task<IActionResult> GetUsersByRole([FromQuery] int roleId)
		{
			try
			{
				var user = await _userService.getUsersByRoleAsync(roleId);
				return Ok(user);
			}
			catch(LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}
		}

		[HttpGet("getUserByUsername")]
		[Authorize(Roles = "Superadmin")]
		public async Task<IActionResult> GetUserByUsername([FromQuery] string username)
		{
			try
			{
				var user = await _userService.getUserByUsername(username);

				if(!user.Any())
				{
					throw new LibraryManagementSystemException("İstifadəçi tapılmadı.");
				}
				else if(user is null)
				{
					throw new LibraryManagementSystemException("Axtarış üçün bir term daxil edilməlidir.");
				}

				return Ok(user);
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}
		}

		[HttpDelete("deleteUser/{userId}")]
		[Authorize(Roles = "Superadmin")]
		public async Task<IActionResult> DeleteUserAsync(int userId)
		{
			try
			{
				var user = await _userService.deleteUserAsync(userId);

				if (!user)
				{
					throw new LibraryManagementSystemException("İstifadəçi tapılmadı.");
				}
				else
				{
					return Ok("İstifadəçi silindi.");
				}
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");

			}
		}
	}
}
