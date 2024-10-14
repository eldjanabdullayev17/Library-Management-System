namespace Library_Management_System.DTOs.CheckoutAndReturn
{
	public class CheckOverdueBooksDTO
	{
		public int? BookId { get; set; }
		public string? BookTitle { get; set; }
		public string DueDate { get; set; }
	}
}
