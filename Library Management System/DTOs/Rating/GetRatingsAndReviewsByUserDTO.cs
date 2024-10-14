namespace Library_Management_System.DTOs.Rating
{
    public class GetRatingsAndReviewsByUserDTO
    {
        public string? UserName { get; set; }
        public List<byte?> Ratings { get; set; } = new List<byte?>();
        public List<string?> Reviews { get; set; } = new List<string?>();
    }
}
