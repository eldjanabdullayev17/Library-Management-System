namespace Library_Management_System.Models
{
	public partial class Rating
	{
		public int Id { get; set; }
		public byte? BookRating { get; set; }
		public int? BookId { get; set; }
		public int? UserId { get; set; }
		public virtual Book? Book { get; set; }
		public virtual User? User { get; set; }

	}
}
