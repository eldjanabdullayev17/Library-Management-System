using System.Net;
using Library_Management_System.DTOs.Report;
using Library_Management_System.Exceptions;
using Library_Management_System.Models;
using Library_Management_System.Services.v1.Interfaces;
using Library_Management_System.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Library_Management_System.Services.v1.Service
{
    public class ReportService : IReportService
    {
        private readonly OnlineLibraryManagementSystemContext _context;
        public ReportService(OnlineLibraryManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<Report> generateRentalStatisticsAsync()
        {
            var totalRentals = await _context.Rentals.CountAsync();
            var activeRentals = await _context.Rentals.Where(r => r.Status == true).CountAsync();
            var returnedBooks = totalRentals - activeRentals;

            var rentalStatistics = new Report
            {
                ReportType = "Rental Statistics",
                GeneratedDate = DateTime.Now,
                ReportData = $" TotalRentals : {totalRentals}, ActiveRentals : {activeRentals}, ReturnedBooks : {returnedBooks}",
                Description = "İyul ayı üçün kirayə statistikası"
            };

            return rentalStatistics;
        }

        public async Task<IEnumerable<Report>> generateUserActivityReportAsync(int userId)
        {
            IsValid.IsValidId(userId);

            var user = await _context.Users.AnyAsync(u => u.Id == userId);

			if (!user)
			{
				throw new LibraryManagementSystemException($"Id-si {userId} olan istifadəçi tapılmadı.");
			}

			var userActivityLog = await _context.LogTables.Where(u => u.UserId == userId).ToListAsync();

            var userActivity = userActivityLog.Select(u => new Report
            {
                Id = u.Id,
				ReportType = "User Activity",
				GeneratedDate = DateTime.Now,
				ReportData = u.LogMessage,
				Description = "Sistemdə istifadəçilərin fəaliyyəti"
			}).ToList();
            
            return userActivity;
        }

        public async Task<IEnumerable<GetBookRentalHistoryDTO>> getBookRentalHistoryAsync(int bookId)
        {
            IsValid.IsValidId(bookId);

            var bookExists = await _context.Books.AnyAsync(b => b.Id == bookId);

            if (!bookExists)
            {
                throw new LibraryManagementSystemException($"Id-si {bookId} olan kitab tapılmadı.");
            }

            var rentalHistory = await _context.Rentals.Where(r => r.BookId == bookId && r.Status != false)
                .Select(r => new GetBookRentalHistoryDTO
                {
                    UserId = r.UserId,
                    RentalDate = r.RentalDate.ToString("yyyy-MM-dd"),
                    DueDate = r.DueDate.ToString("yyyy-MM-dd")
                }).ToListAsync();

            if (!rentalHistory.Any())
            {
                throw new LibraryManagementSystemException("Kitab kirayə götürülməyib.");
            }

            return rentalHistory;
        }

        public async Task<IEnumerable<GetMostReadBooksDTO>> getMostReadBooksAsync()
        {
            return await _context.Rentals
                .GroupBy(r => r.BookId)
                .OrderByDescending(r => r.Count())
                .Select(r => new GetMostReadBooksDTO
                {
                    BookId = (int)r.Key,
                    BookTitle = r.Select(b => b.Book.BookTitle).FirstOrDefault(),
                    ReadCount = r.Count()
                })
                .Take(3)
                .ToListAsync();
        }

        public async Task<IEnumerable<GetUserLoginHistoryDTO>> getUserLoginHistoryAsync(int userId)
        {
            IsValid.IsValidId(userId);

            var user = await _context.Users.AnyAsync(u => u.Id == userId);

            if (!user)
            {
                throw new LibraryManagementSystemException($"Id-si {userId} olan istifadəçi tapılmadı.");
            }

            var userLogins = await _context.UserLogins
                .Include(u => u.User)
                .Where(u => u.UserId == userId).ToListAsync();

            if (!userLogins.Any())
            {
                throw new LibraryManagementSystemException($" ID-si {userId} olan istifadəçi login olmayib.");
            }
            else
            {
                return userLogins.Select(u => new GetUserLoginHistoryDTO
                {
                    UserId = userId,
                    UserName = u.User.Username,
                    UserLoginDate = u.UserLoginDate
                }).ToList();
            }
        }
    }
}
