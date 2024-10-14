namespace Library_Management_System.DTOs.Notification
{
	public class GetNotificationsByUserDTO
	{
		public int Id { get; set; }

		public string? UserName { get; set; }

		public string NotificationMessage { get; set; } = null!;

		public DateTime? NotificationDate { get; set; }
	}
}
