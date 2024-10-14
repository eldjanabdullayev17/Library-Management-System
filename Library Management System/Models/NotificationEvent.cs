namespace Library_Management_System.Models
{
	public partial class NotificationEvent
	{
		public int Id { get; set; }

		public string NotificationMessage { get; set; } = null!;

		public DateTime? NotificationDate { get; set; }

		public int? EventId { get; set; }

		public virtual Event? Event { get; set; }

	}
}
