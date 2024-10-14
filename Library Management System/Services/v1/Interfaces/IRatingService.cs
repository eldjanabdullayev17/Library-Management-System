using Library_Management_System.DTOs.Rating;

namespace Library_Management_System.Services.v1.Interfaces
{
    public interface IRatingService
    {
        // Kitaba yeni reytinq əlavə edir.
        Task<AddRatingDTO> addRatingAsync(AddRatingDTO rating);

        // Mövcud reytinqi yeniləyir.
        Task<bool> updateRatingAsync(int ratingId, UpdateRatingDTO newRtaing);

        // Mövcud reytinqi silir.
        Task<bool> deleteRatingAsync(int ratingId);

        // Kitabın orta reytinqini əldə edir.
        Task<double> getAverageRatingAsync(int bookId);

        // İstifadəçinin kitab üçün verdiyi reytinqi əldə edir.
        Task<GetUserRatingDTO> getUserRatingAsync(int userId, int bookId);


        // Yeni reytinq və rəy əlavə edir.
        Task<AddRatingAndReviewDTO> addRatingAndReviewAsync(AddRatingAndReviewDTO ratingAndReview);

        // Bir kitab üçün bütün reytinqləri və rəyləri gətirir.
        Task<GetRatingsAndReviewsByBookDTO> getRatingsAndReviewsByBookAsync(int bookId);

        // Bir istifadəçi tərəfindən verilmiş bütün reytinqləri və rəyləri gətirir.
        Task<GetRatingsAndReviewsByUserDTO> getRatingsAndReviewsByUserAsync(int userId);

        // Mövcud reytinqi və rəyi yeniləyir.
        Task<bool> updateRatingAndReviewAsync(int bookId, int userId, UpdateRatingAndReviewDTO newUpdateRatingAndReview);

        // Mövcud reytinqi və rəyi silir.
        Task<bool> deleteRatingAndReviewAsync(int bookId, int userId);
    }
}
