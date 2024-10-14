namespace Library_Management_System.DTOs.Rating
{
	public class GetUserRatingDTO
	{
		public string? UserName { get; set; }
		public string? BookTitle { get; set; }
		public byte? Rating { get; set; }
	}
}
