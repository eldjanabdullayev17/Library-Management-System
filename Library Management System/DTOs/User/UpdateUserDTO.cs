namespace Library_Management_System.DTOs.User
{
	public class UpdateUserDTO
	{
		public string Firstname { get; set; } = null!;

		public string Lastname { get; set; } = null!;

		public string Username { get; set; } = null!;

		public string Password { get; set; } = null!;
		public int RoleId { get; set; }

	}
}
