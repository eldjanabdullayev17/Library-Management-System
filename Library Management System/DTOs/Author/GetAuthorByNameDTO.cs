namespace Library_Management_System.DTOs.Author
{
	public class GetAuthorByNameDTO
	{
		public int Id { get; set; }
		public string AuthorName { get; set; } = null!;

		public string AuthorSurname { get; set; } = null!;

		public DateTime? AuthorBirthDate { get; set; }

		public string Nationality { get; set; } = null!;

		public string AuthorBiography { get; set; } = null!;

		public string AuthorImg { get; set; } = null!;

		public int NumberOfBooks { get; set; }
	}
}
