﻿namespace Library_Management_System.DTOs.Book
{
	public class AddBookDTO
	{
		public string BookTitle { get; set; } = null!;

		public double BookPrice { get; set; }

		public string BookImg { get; set; } = null!;

		public int BookPage { get; set; }

		public DateTime? BookPublicationYear { get; set; }

		public int BookInventoryCount { get; set; }

		public int? AuthorId { get; set; }

		public int? CategoryId { get; set; }

		public int? LanguageId { get; set; }
	}
}
