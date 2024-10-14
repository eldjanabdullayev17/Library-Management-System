namespace Library_Management_System.DTOs.Report
{
	public class GetBookRentalHistoryDTO
	{
		public int? UserId { get; set; }
		public string? RentalDate { get; set; }
		public string? DueDate { get; set; }
	}
}
