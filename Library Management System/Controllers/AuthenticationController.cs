using Library_Management_System.DTOs.Registration;
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
	public class AuthenticationController : ControllerBase
	{
		private readonly IAuthenticationService _authenticationService;
		public AuthenticationController(IAuthenticationService authenticationService)
		{
			_authenticationService = authenticationService;
		}

		[HttpPost("registerUser")]
		public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterUserDTO registerDto)
		{
			try
			{
				await _authenticationService.registerUserAsync(registerDto);
				return Ok("İstifadəçi uğurla qeydiyyatdan keçdi.");
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message : {ex.Message}");
			}

		}

		[HttpPost("login")]
		public async Task<IActionResult> LoginAsync([FromBody] LoginDTO loginUser)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("Yanlış məlumat daxil edilib.");
			}

			try
			{
				var token = await _authenticationService.loginAsync(loginUser);
				return Ok(new { UserName = loginUser.UserName, Token = token });
			}
			catch (LibraryManagementSystemException ex)
			{
				return Unauthorized($"Giriş uğursuz oldu: {ex.Message}");
			}
		}

		[HttpPost("logout/{userId}")]
		public async Task<IActionResult> LogoutAsync(int userId)
		{
			try
			{
				var success = await _authenticationService.logoutAsync(userId);
				if (success)
				{
					return Ok("İstifadəçi uğurla çıxış etdi.");
				}
				else
				{
					throw new LibraryManagementSystemException("Uğursuz nəticə.");
				}
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message : {ex.Message}");
			}

		}
	}
}
