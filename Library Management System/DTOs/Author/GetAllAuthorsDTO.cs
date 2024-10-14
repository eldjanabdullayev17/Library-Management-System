﻿namespace Library_Management_System.DTOs.Author
{
	public class GetAllAuthorsDTO
	{
		public string AuthorName { get; set; } = null!;

		public string AuthorSurname { get; set; } = null!;

		public DateTime? AuthorBirthDate { get; set; }

		public string Nationality { get; set; } = null!;

		public string AuthorBiography { get; set; } = null!;

	}
}
