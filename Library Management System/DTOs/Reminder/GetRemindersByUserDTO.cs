namespace Library_Management_System.DTOs.Reminder
{
	public class GetRemindersByUserDTO
	{
		public int ReservationId { get; set; }
		public string UserName { get; set; } = null!;

		public List<string> Reminders { get; set; } = new List<string>();
	}
}
