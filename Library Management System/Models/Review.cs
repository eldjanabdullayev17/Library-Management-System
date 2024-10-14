namespace Library_Management_System.Models
{
	public partial class Review
	{
		public int Id { get; set; }
		public string? ReviewText { get; set; }
		public DateTime? ReviewDate { get; set; }
		public int? BookId { get; set; }
		public int? UserId { get; set; }
		public virtual Book? Book { get; set; }
		public virtual User? User { get; set; }
	}
}
