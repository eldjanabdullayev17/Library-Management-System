using Library_Management_System.DTOs.Review;

namespace Library_Management_System.Services.v1.Interfaces
{
    public interface IReviewService
    {
        // Kitaba yeni rəy əlavə edir.
        Task<AddReviewDTO> addReviewAsync(AddReviewDTO review);

        // Mövcud rəyi yeniləyir.
        Task<bool> updateReviewAsync(int reviewId, UpdateReviewDTO newReview);

        // Mövcud rəyi silir.
        Task<bool> deleteReviewAsync(int reviewId);

        // Kitab üçün bütün rəyləri əldə edir.
        Task<GetAllReviewsForBookDTO> getAllReviewsForBookAsync(int bookId);

        // İstifadəçinin kitab üçün yazdığı rəyi əldə edir
        Task<IEnumerable<GetUserReviewDTO>> getUserReviewAsync(int userId, int bookId);

    }
}
