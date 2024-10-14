using Library_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.DTOs.CheckoutAndReturn
{
	public class ReturnList
	{
		public static List<Models.Rental> List()
		{
			OnlineLibraryManagementSystemContext context = new OnlineLibraryManagementSystemContext();

			var overdueBooks = context.Rentals
				.Include(r => r.Book)
				.Include(r => r.User)
				.Where(r => r.Status).ToList();

			return overdueBooks;
		}

	}
}
