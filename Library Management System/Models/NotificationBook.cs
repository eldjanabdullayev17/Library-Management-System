namespace Library_Management_System.Models
{
	public partial class NotificationBook
	{
		public int Id { get; set; }

		public int? UserId { get; set; }

		public string NotificationMessage { get; set; } = null!;

		public DateTime? NotificationDate { get; set; }

		public int? BookId { get; set; }

		public virtual Book? Book { get; set; }

		public virtual User? User { get; set; }
	}
}
