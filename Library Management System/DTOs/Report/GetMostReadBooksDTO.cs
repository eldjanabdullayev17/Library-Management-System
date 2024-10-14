namespace Library_Management_System.DTOs.Report
{
	public class GetMostReadBooksDTO
	{
		public int BookId { get; set; }
		public string? BookTitle { get; set; }
		public int ReadCount { get; set; }
	}
}
