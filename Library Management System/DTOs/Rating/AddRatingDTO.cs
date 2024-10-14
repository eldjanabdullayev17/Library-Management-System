namespace Library_Management_System.DTOs.Rating
{
	public class AddRatingDTO
	{
		public byte? BookRating { get; set; }
		public int? BookId { get; set; }
		public int? UserId { get; set; }
	}
}
