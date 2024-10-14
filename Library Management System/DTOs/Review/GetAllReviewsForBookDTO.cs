namespace Library_Management_System.DTOs.Review
{
	public class GetAllReviewsForBookDTO
	{
		public int? BookId { get; set; }
		public string? BookTitle { get; set; }
		public List<string> Reviews { get; set; } = new List<string>();
	}
}
