using Library_Management_System.DTOs.Report;
using Library_Management_System.Models;

namespace Library_Management_System.Services.v1.Interfaces
{
    public interface IReportService
    {
        // Kirayə statistikalarını yaradır.
        Task<Report> generateRentalStatisticsAsync();

        // Ən çox oxunan kitabları əldə edir
        Task<IEnumerable<GetMostReadBooksDTO>> getMostReadBooksAsync();

        // İstifadəçi fəaliyyətinin hesabatını yaradır.
        Task<IEnumerable<Report>> generateUserActivityReportAsync(int userId);

        // Kitabın kirayə tarixçəsini əldə edir.
        Task<IEnumerable<GetBookRentalHistoryDTO>> getBookRentalHistoryAsync(int bookId);

        // İstifadəçinin giriş tarixçəsini əldə edir.
        Task<IEnumerable<GetUserLoginHistoryDTO>> getUserLoginHistoryAsync(int userId);
    }
}
