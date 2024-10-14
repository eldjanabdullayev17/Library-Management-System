using Library_Management_System.DTOs.CheckoutAndReturn;
using Library_Management_System.Exceptions;
using Library_Management_System.Models;
using Library_Management_System.Services.v1.Interfaces;
using Library_Management_System.Validation;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Services.v1.Service
{
    public class CheckoutAndReturnsService : ICheckoutAndReturnsService
    {
        private readonly OnlineLibraryManagementSystemContext _context;
        public CheckoutAndReturnsService(OnlineLibraryManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CheckOverdueBooksDTO>> checkOverdueBooksAsync(DateTime currentDate)
        {
            var overdueBooks = await _context.Rentals.Include(x => x.Book)
                .Where(x => x.DueDate < currentDate && !x.Status)
                .Select(x => new CheckOverdueBooksDTO
                {
                    BookId = x.BookId,
                    BookTitle = x.Book.BookTitle,
                    DueDate = x.DueDate.ToShortDateString(),

                }).ToListAsync();

            return overdueBooks;
        }

        public async Task<bool> updateBookStatusAsync(int rentalId)
        {
            IsValid.IsValidId(rentalId);

            var rental = await _context.Rentals.FindAsync(rentalId);

            if (rental != null && rental.Status != false)
            {
                rental.Status = false;
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> sendOverdueNoticesAsync()
        {
            var overdueBooks = await _context.Rentals
                .Include(r => r.Book)
                .Include(r => r.User)
                .Where(r => r.Status).ToListAsync();

			foreach (var rental in overdueBooks)
            {
                if (rental.BookId.HasValue && rental.Status)
                {
                    var message = $"Hörmətli {rental.User?.Username}, zəhmət olmasa '{rental.Book?.BookTitle}' adlı kitabı geri qaytarın.";

                    Console.WriteLine(message);
                }
            }

            return true;
        }

        public async Task logReturnEventAsync(int bookId, int userId, DateTime returnDate)
        {
            IsValid.IsValidId(bookId);
            IsValid.IsValidId(userId);

            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            var bookExists = await _context.Books.AnyAsync(u => u.Id == bookId);

            if (!bookExists && !userExists)
            {
                throw new LibraryManagementSystemException($"Id-si {bookId} olan kitab və Id-si {userId} olan istifadəçi tapılmadı.");
            }

            if (!bookExists)
            {
                throw new LibraryManagementSystemException($"Id-si {bookId} olan kitab tapılmadı.");
            }

            if (!userExists)
            {
                throw new LibraryManagementSystemException($"Id-si {userId} olan istifadəçi tapılmadı.");
            }

            var logReturnEvent = new LogTable
            {
                UserId = userId,
                BookId = bookId,
                LogMessage = "Kitabın geri qaytarılması zamanı hadisə baş verdi.",
                LogDate = returnDate

			};
        }
    }
}
