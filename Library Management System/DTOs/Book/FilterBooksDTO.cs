namespace Library_Management_System.DTOs.Book
{
	public class FilterBooksDTO
	{
		public int Id { get; set; }
		public string BookTitle { get; set; } = null!;
		public double BookPrice { get; set; }
		public string? Author { get; set; }
		public string? Category { get; set; }
	}

	public class QueryObject
	{
		public string? Author { get; set; } = null;
		public string? Category { get; set; } = null;
	}
}
