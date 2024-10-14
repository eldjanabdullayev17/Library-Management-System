using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Library_Management_System.Exceptions;
using Library_Management_System.DTOs.Book;
using Microsoft.AspNetCore.Authorization;
using Library_Management_System.Services.v1.Interfaces;

namespace Library_Management_System.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class BookController : ControllerBase
	{
		private readonly IBookService _bookService;
		public BookController(IBookService bookService)
		{
			_bookService = bookService;
		}

		[HttpPost("addBook")]
		//[Authorize(Roles = "Superadmin,Admin")]
		[Authorize]
		public async Task<IActionResult> AddBookAsync([FromBody] AddBookDTO book)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var newBook = await _bookService.addBookAsync(book);
				return Ok(newBook);
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}
		}

		[HttpPut("updateBook/{bookId}")]
		//[Authorize(Roles = "Superadmin,Admin")]
		[Authorize]
		public async Task<IActionResult> UpdateBookAsync(int bookId, [FromBody] UpdateBookDTO newBook)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var book = await _bookService.updateBookAsync(bookId, newBook);
				if (!book)
				{
					throw new LibraryManagementSystemException("Kitab tapılmadı.");
				}
				else
				{
					return Ok("Kitabın məlumatları dəyişdirildi.");
				}
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}
		}

		[HttpGet("getAllBooks")]
		[Authorize(Roles = "Superadmin,Admin")]
		public async Task<IActionResult> GetAllBooksAsync()
		{
			var books = await _bookService.getAllBooksAsync();
			return Ok(books);
		}

		[HttpGet("getBookById/{bookId}")]
		[Authorize(Roles = "Superadmin,Admin")]
		public async Task<IActionResult> GetBookByIdAsync(int bookId)
		{
			try
			{
				var book = await _bookService.getBookByIdAsync(bookId);
				return Ok(book);

			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}
		}

		[HttpGet("filterBooks")]
		public async Task<IActionResult> FiltersBooksAsync([FromQuery] QueryObject query)
		{
			var books = await _bookService.filterBooksAsync(query);
			return Ok(books);
		}

		[HttpGet("getBookInventory")]
		[Authorize(Roles = "Superadmin,Admin")]
		public async Task<IActionResult> GetBookInventory()
		{
			var bookInventory = await _bookService.getBookInventoryAsync();
			return Ok(bookInventory);
		}

		[HttpGet("searchBooks")]
		public async Task<IActionResult> SearhBooksAsync([FromQuery] string keyword)
		{
			try
			{
				var books = await _bookService.searchBooksAsync(keyword);

				if (!books.Any())
				{
					throw new LibraryManagementSystemException("Heç bir kitab tapılmadı.");
				}
				
				return Ok(books);
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}
		}

		[HttpDelete("deleteBook/{bookId}")]
		//[Authorize(Roles = "Superadmin,Admin")]
		[Authorize]
		public async Task<IActionResult> DeleteBookAsync(int bookId)
		{
			try
			{
				var book = await _bookService.deleteBookAsync(bookId);
				if (!book)
				{
					throw new LibraryManagementSystemException("Kitab tapılmadı.");
				}
				else
				{
					return Ok("Kitab silindi.");
				}
			}
			catch (LibraryManagementSystemException ex)
			{
				return BadRequest($"Error Message: {ex.Message}");
			}
		}


	}
}
