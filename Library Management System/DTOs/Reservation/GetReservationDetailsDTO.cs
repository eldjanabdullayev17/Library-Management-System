﻿namespace Library_Management_System.DTOs.Reservation
{
	public class GetReservationDetailsDTO
	{
		public int ReservationId { get; set; }

		public string? BookTitle { get; set; }

		public string? UserName { get; set; }

		public DateTime? ReservationDate { get; set; }

		public DateTime? ExpirationDate { get; set; }

	}
}
