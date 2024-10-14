namespace Library_Management_System.DTOs.Review
{
	public class GetUserReviewDTO
	{
		public string? UserName { get; set; }
		public string? BookTitle { get; set; }
		public string? ReviewText { get; set; }
		public DateTime? ReviewDate { get; set; } 

	}
}
