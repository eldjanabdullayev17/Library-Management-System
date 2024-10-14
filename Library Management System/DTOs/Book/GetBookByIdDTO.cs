﻿namespace Library_Management_System.DTOs.Book
{
	public class GetBookByIdDTO
	{
		public int Id { get; set; }

		public string BookTitle { get; set; } = null!;

		public double BookPrice { get; set; }

		public string BookImg { get; set; } = null!;

		public int BookPage { get; set; }

		public DateTime? BookPublicationYear { get; set; }

		public int BookInventoryCount { get; set; }

		public string? Author { get; set; }

		public string? Category { get; set; }

		public string Language { get; set; }


	}
}
