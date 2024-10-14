using Library_Management_System.DTOs.Reservation;
using Library_Management_System.Models;
using Library_Management_System.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Library_Management_System.Validation;
using Library_Management_System.Services.v1.Interfaces;

namespace Library_Management_System.Services.v1.Service
{
    public class ReservationService : IReservationService
    {
        private readonly OnlineLibraryManagementSystemContext _context;
        public ReservationService(OnlineLibraryManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<AddReservationDTO> addReservationAsync(AddReservationDTO reservation)
        {
       
            IsValid.IsValidId(reservation.UserId);
            IsValid.IsValidId(reservation.BookId);

            var book = await _context.Books.AnyAsync(r => r.Id == reservation.BookId);
            var user = await _context.Users.AnyAsync(r => r.Id == reservation.UserId);

            if (!book && !user)
            {
                throw new LibraryManagementSystemException($"Id-si {reservation.BookId} olan kitab və Id-si {reservation.UserId} olan istifadəçi tapılmadı.");
            }

            if (!book)
            {
                throw new LibraryManagementSystemException($"Id-si {reservation.BookId} olan kitab tapılmadı.");
            }

            if (!user)
            {
                throw new LibraryManagementSystemException($"Id-si {reservation.UserId} olan istifadəçi tapılmadı.");
            }

            var newReservation = new Reservation
            {
                BookId = reservation.BookId,
                UserId = reservation.UserId,
                ReservationDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddDays(14),
                Active = true
            };

            var reservationLog = new LogTable
            {
                UserId = reservation.UserId,
                BookId = reservation.BookId,
                LogMessage = $"İstifadəçi yeni kitab rezervasiya etdi.",
                LogDate = DateTime.Now,
            };

            await _context.LogTables.AddAsync(reservationLog);
            await _context.Reservations.AddAsync(newReservation);
            await _context.SaveChangesAsync();
            return reservation;
        }

        public async Task<bool> cancelReservationAsync(int reservationId)
        {
            IsValid.IsValidId(reservationId);

            var reservation = await _context.Reservations
                .SingleOrDefaultAsync(r => r.Id == reservationId && r.Active != false);

            if (reservation != null)
            {
                reservation.Active = false;
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> checkAvailabilityAsync(int bookId, DateTime startDate, DateTime endDate)
        {
            IsValid.IsValidId(bookId);

            var book = await _context.Books.AnyAsync(r => r.Id == bookId);

            if (!book)
            {
                throw new LibraryManagementSystemException($"Id-si {bookId} olan kitab tapılmadı.");
            }

            if (startDate > DateTime.Now)
            {
                throw new LibraryManagementSystemException("Başlanğıc tarixi düzgün deyil.");
            }

            if (startDate >= endDate)
            {
                throw new LibraryManagementSystemException("Başlanğıc tarixi bitmə tarixindən böyük ola bilməz.");
            }

            var isRented = await _context.Reservations
                .Where(r => r.BookId == bookId &&
                            r.ReservationDate < endDate && r.ExpirationDate > startDate)
                .AnyAsync();

            return !isRented;
        }

        public async Task<IEnumerable<GetBookReservationsDTO>> getBookReservationsAsync(int bookId)
        {
            IsValid.IsValidId(bookId);

            var book = await _context.Books.AnyAsync(r => r.Id == bookId);

            if (!book)
            {
                throw new LibraryManagementSystemException($"Id-si {bookId} olan kitab tapılmadı.");
            }

            return await _context.Reservations.Include(x => x.Book)
            .Where(r => r.BookId == bookId && r.Active != false)
            .Select(r => new GetBookReservationsDTO
            {
                ReservationId = r.Id,
                BookTitle = r.Book.BookTitle,
                ReservationDate = r.ReservationDate,
                ExpirationDate = r.ExpirationDate
            }).ToListAsync();
        }

        public async Task<GetReservationDetailsDTO> getReservationDetailsAsync(int reservationId)
        {
            IsValid.IsValidId(reservationId);

            var reservation = await _context.Reservations
                .Include(x => x.Book)
                .Include(x => x.User)
                .SingleOrDefaultAsync(x => x.Id == reservationId && x.Active != false);

            if (reservation != null)
            {
                return new GetReservationDetailsDTO
                {
                    ReservationId = reservation.Id,
                    BookTitle = reservation.Book.BookTitle,
                    UserName = reservation.User.Username,
                    ReservationDate = reservation.ReservationDate,
                    ExpirationDate = reservation.ExpirationDate
                };
            }
            else
            {
                throw new LibraryManagementSystemException("Rezervasiya tapılmadı.");
            }
        }

        public async Task<IEnumerable<GetUserReservationsDTO>> getUserReservationsAsync(int userId)
        {
            IsValid.IsValidId(userId);

            var user = await _context.Users.AnyAsync(r => r.Id == userId);

            if (!user)
            {
                throw new LibraryManagementSystemException($"Id-si {userId} olan istifadəçi tapılmadı.");
            }

            return await _context.Reservations.Include(x => x.User)
            .Where(r => r.UserId == userId && r.Active != false)
            .Select(r => new GetUserReservationsDTO
            {
                ReservationId = r.Id,
                UserName = r.User.Username,
                ReservationDate = r.ReservationDate,
                ExpirationDate = r.ExpirationDate
            }).ToListAsync();
        }

        public async Task<bool> updateReservationAsync(int reservationId, UpdateReservationDTO newReservation)
        {
            IsValid.IsValidId(reservationId);

            var reservation = await _context.Reservations
                .SingleOrDefaultAsync(r => r.Id == reservationId);

            if (reservation is null)
            {
                return false;
            }
            else
            {
                reservation.ReservationDate = newReservation.ReservationDate;
                reservation.ExpirationDate = newReservation.ExpirationDate;
                await _context.SaveChangesAsync();
                return true;
            }
        }
    }
}
