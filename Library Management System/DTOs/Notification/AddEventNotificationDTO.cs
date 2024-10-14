namespace Library_Management_System.DTOs.Notification
{
	public class AddEventNotificationDTO
	{
		public string NotificationMessage { get; set; } = null!;

		public DateTime? NotificationDate { get; set; }

		public int? EventId { get; set; }
	}
}
