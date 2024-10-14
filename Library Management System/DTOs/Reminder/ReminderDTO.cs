namespace Library_Management_System.DTOs.Reminder
{
	public class ReminderDTO
	{
		public int ReminderId { get; set; }

		public int? BookId { get; set; }

		public int? UserId { get; set; }

		public DateTime? ReminderDate { get; set; }

		public string ReminderMessage { get; set; } = null!;
	}
}
