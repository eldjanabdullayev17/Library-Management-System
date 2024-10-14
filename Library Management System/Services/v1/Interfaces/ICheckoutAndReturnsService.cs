using Library_Management_System.DTOs.CheckoutAndReturn;
using Library_Management_System.Models;

namespace Library_Management_System.Services.v1.Interfaces
{
    public interface ICheckoutAndReturnsService
    {
        // Geri qaytarılma tarixi keçmiş kitabları yoxlayır.
        Task<IEnumerable<CheckOverdueBooksDTO>> checkOverdueBooksAsync(DateTime currentDate);

        // Kitabın statusunu (məsələn, “qaytarılıb”) yeniləyir.
        Task<bool> updateBookStatusAsync(int rentalİd);

        // Geri qaytarılmayan kitablar üçün istifadəçilərə bildiriş göndərir.
        Task<bool> sendOverdueNoticesAsync();

        // Kitabın geri qaytarılması zamanı baş verən hadisəni qeyd edir.
        Task logReturnEventAsync(int bookId, int userId, DateTime returnDate);


    }
}
