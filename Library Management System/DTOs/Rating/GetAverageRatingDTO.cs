namespace Library_Management_System.DTOs.Rating
{
	public class GetAverageRatingDTO
	{
		public int BookId { get; set; }
		public string? BookTitle { get; set; }
		public double BookAvarageRating { get; set; }
	}
}
