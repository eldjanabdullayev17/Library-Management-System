namespace Library_Management_System.DTOs.Reservation
{
	public class GetUserReservationsDTO
	{
		public int ReservationId { get; set; }

		public string? UserName { get; set; }

		public DateTime? ReservationDate { get; set; }

		public DateTime? ExpirationDate { get; set; }
	}
}
