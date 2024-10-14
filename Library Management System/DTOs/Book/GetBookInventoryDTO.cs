namespace Library_Management_System.DTOs.Book
{
	public class GetBookInventoryDTO
	{
		public int Id { get; set; }
		public string BookTitle { get; set; } = null!;
		public int BookInventoryCount { get; set; }
	}
}
