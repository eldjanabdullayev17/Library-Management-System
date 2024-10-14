using Library_Management_System.DTOs.Reservation;

namespace Library_Management_System.Services.v1.Interfaces
{
    public interface IReservationService
    {

        // İstifadəçi tərəfindən kitab üçün yeni rezervasiya əlavə edir.
        Task<AddReservationDTO> addReservationAsync(AddReservationDTO reservation);

        // Mövcud rezervasiyanı yeniləyir (məsələn, tarixləri dəyişdirir).
        Task<bool> updateReservationAsync(int reservationId, UpdateReservationDTO newReservation);

        // Kitabın müəyyən tarixlərdə mövcud olub olmadığını yoxlayır.
        Task<bool> checkAvailabilityAsync(int bookId, DateTime startDate, DateTime endDate);

        // Mövcud rezervasiyanı ləğv edir.
        Task<bool> cancelReservationAsync(int reservationId);

        // Müəyyən rezervasiya haqqında ətraflı məlumat əldə edir.
        Task<GetReservationDetailsDTO> getReservationDetailsAsync(int reservationId);

        // İstifadəçinin bütün aktiv rezervasiyalarını əldə edir.
        Task<IEnumerable<GetUserReservationsDTO>> getUserReservationsAsync(int userId);

        // Kitab üçün bütün aktiv rezervasiyaları əldə edir.
        Task<IEnumerable<GetBookReservationsDTO>> getBookReservationsAsync(int bookId);
    }
}
