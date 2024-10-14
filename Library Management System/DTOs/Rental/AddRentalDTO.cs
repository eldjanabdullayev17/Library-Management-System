namespace Library_Management_System.DTOs.Rental
{
	public class AddRentalDTO
	{
		public DateTime RentalDate { get; set; }

		public DateTime DueDate { get; set; }

		public int? BookId { get; set; }

		public int? UserId { get; set; }
	}
}
