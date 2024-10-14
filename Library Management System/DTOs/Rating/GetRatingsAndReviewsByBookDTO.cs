namespace Library_Management_System.DTOs.Rating
{
    public class GetRatingsAndReviewsByBookDTO
    {
        public string? BookTitle { get; set; }
        public List<byte?> Ratings { get; set; } = new List<byte?>();
        public List<string?> Reviews { get; set; } = new List<string?>();
    }
}
