namespace Library_Management_System.DTOs.Reservation
{
	public class AddReservationDTO
	{
		public int? BookId { get; set; }

		public int? UserId { get; set; }

		public DateTime? ReservationDate { get; set; } = DateTime.Now;

		public DateTime? ExpirationDate { get; set; } = DateTime.Now.AddDays(14);

	}
}
