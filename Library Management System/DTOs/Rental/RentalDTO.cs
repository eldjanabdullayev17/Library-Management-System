namespace Library_Management_System.DTOs.Rental
{
	public class RentalDTO
	{
		public int RentalId { get; set; }

		public string? BookTitle { get; set; }

		public string? UserName { get; set; }

		public DateTime? RentalDate { get; set; }

		public DateTime? DueDate { get; set; }
	}
}
