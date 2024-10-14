namespace Library_Management_System.DTOs.User
{
	public class UserDTO
	{
		public int Id { get; set; }

		public string Firstname { get; set; } = null!;

		public string Lastname { get; set; } = null!;

		public string Username { get; set; } = null!;

		public string Password { get; set; } = null!;

		public string? Role { get; set; }

		public bool Active { get; set; }
	}
}
