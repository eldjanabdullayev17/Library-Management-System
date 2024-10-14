using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Library_Management_System.Exceptions;
using Microsoft.AspNetCore.Components.Forms;
using Library_Management_System.DTOs.Author;
using Microsoft.AspNetCore.Authorization;
using Library_Management_System.Models;
using Library_Management_System.Services.v1.Interfaces;

namespace Library_Management_System.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class AuthorController : ControllerBase
	{
		private readonly IAuthorService _authorService;
		public AuthorController(IAuthorService authorService)
		{
			_authorService = authorService;
		}

		[HttpPost("addAuthor")]
		[Authorize(Roles = "Superadmin")]
		public async Task<IActionResult> AddAuthorAsync([FromBody] AddAuthorDTO author)
		{

			if (!ModelState.IsValid)
			{
				return BadRequest("Yanlış məlumat daxil edilib.");
			}

			try
			{
				var newAuthor = await _authorService.addAuthorAsync(author);
				return Ok(newAuthor);
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}


		}

		[HttpPut("updateAuthor/{authorId}")]
		[Authorize(Roles = "Superadmin,Admin")]
		public async Task<IActionResult> UpdateAuthorAsync(int authorId, [FromBody] UpdateAuthorDTO newAuthor)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var author = await _authorService.updateAuthorAsync(authorId, newAuthor);
				if (!author)
				{

					throw new LibraryManagementSystemException("Müəllif tapılmadı.");
				}
				else
				{
					return Ok("Müəllifin məlumatları dəyişdirildi.");
				}

			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}
		}

		[HttpGet("getAllAuthors")]
		[Authorize(Roles = "Superadmin,Admin")]
		public async Task<IActionResult> GetAllAuthorsAsync()
		{
			var authors = await _authorService.getAllAuthorsAsync();
			return Ok(authors);
		}

		[HttpGet("getAuthorById/{authorId}")]
		[Authorize(Roles = "Superadmin,Admin")]
		public async Task<IActionResult> GetAuthorByIdAsync(int authorId)
		{
			try
			{
				var author = await _authorService.getAuthorByIdAsync(authorId);
				return Ok(author);
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}
		}

		[HttpGet("getAuthorByName")]
		[Authorize(Roles = "Superadmin,Admin")]
		public async Task<IActionResult> GetAuthorByNameAsync([FromQuery] string author)
		{
			try
			{
				var a = await _authorService.getAuthorByNameAsync(author);

				if (!a.Any())
				{
					throw new LibraryManagementSystemException("Müəllif tapılmadı.");
				}

				return Ok(a);

			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}
		}

		[HttpGet("getBooksByAuthor")]
		[Authorize(Roles = "Superadmin,Admin")]
		public async Task<IActionResult> GetBooksByAuthorAsync([FromQuery] int authorId)
		{
			try
			{
				var authors = await _authorService.getBooksByAuthorAsync(authorId);
				return Ok(authors);
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}

		}

		[HttpDelete("deleteAuthor/{authorId}")]
		[Authorize(Roles = "Superadmin")]
		public async Task<IActionResult> DeleteAuthorAsync(int authorId)
		{
			try
			{
				var author = await _authorService.deleteAuthorAsync(authorId);
				if (!author)
				{
					throw new LibraryManagementSystemException("Müəllif tapılmadı.");
				}
				else
				{
					return Ok("Müəllif silindi.");
				}
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");

			}
		}

		[HttpDelete("removeBookFromAuthor")]
		[Authorize(Roles = "Superadmin")]
		public async Task<IActionResult> RemoveBookFromAuthor([FromQuery] int authorId)
		{
			try
			{
				var authors = await _authorService.removeBookFromAuthorAsync(authorId);
				if (!authors)
				{
					throw new LibraryManagementSystemException("Müəllifin kitabı yoxdur.");
				}
				else
				{
					return Ok("Müəllifin kitabları silindi");
				}
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}
		}
	}
}
