namespace Library_Management_System.DTOs.Author
{
	public class GetBooksByAuthorDTO
	{
		public int AuthorId { get; set; } // Author Id
		public string? Author {  get; set; }
		public List<string?> Books { get; set; } = new List<string?>();
	}
}
