namespace Library_Management_System.DTOs.Notification
{
	public class AddNewBookNotificationDTO
	{
		public int? UserId { get; set; }

		public string NotificationMessage { get; set; } = null!;

		public DateTime? NotificationDate { get; set; }

		public int? BookId { get; set; }
	}
}
