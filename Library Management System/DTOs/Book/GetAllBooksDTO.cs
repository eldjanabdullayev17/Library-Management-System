namespace Library_Management_System.DTOs.Book
{
	public class GetAllBooksDTO
	{
		public int Id { get; set; }
		public string? BookTitle { get; set; }
		public double BookPrice { get; set; }
		public DateTime? BookPublicationYear { get; set; }
		public string? Author { get; set; }
		public string? Category { get; set; }
		public string? Language { get; set; }
	}
}
