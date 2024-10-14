using Library_Management_System.DTOs.Rental;
using Library_Management_System.Exceptions;
using Library_Management_System.Models;
using Library_Management_System.Services.v1.Interfaces;
using Library_Management_System.Validation;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Services.v1.Service
{
    public class RentalService : IRentalService
    {
        private readonly OnlineLibraryManagementSystemContext _context;
        public RentalService(OnlineLibraryManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<AddRentalDTO> addRentalAsync(AddRentalDTO rental)
        {
            IsValid.IsValidId(rental.BookId);
            IsValid.IsValidId(rental.UserId);

            if (rental.RentalDate > rental.DueDate)
            {
                throw new LibraryManagementSystemException("Kirayə götürülmə vaxtı son tarixdən böyük ola bilməz.");
            }

            var newRental = new Rental
            {
                BookId = rental.BookId,
                UserId = rental.UserId,
                RentalDate = rental.RentalDate,
                DueDate = rental.DueDate,
                Status = true
            };

            await _context.Rentals.AddAsync(newRental);
            await _context.SaveChangesAsync();
            return rental;
        }

        public async Task<bool> updateRentalAsync(int rentalId, UpdateRentalDTO newRental)
        {
            IsValid.IsValidId(rentalId);

            if (newRental.RentalDate > newRental.DueDate)
            {
                throw new LibraryManagementSystemException("Kirayə götürülmə vaxtı son tarixdən böyük ola bilməz.");
            }

            var rental = await _context.Rentals.FindAsync(rentalId);

            if (rental != null)
            {
                rental.DueDate = newRental.DueDate;
                rental.RentalDate = newRental.DueDate;

                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<RentalDTO> getRentalByIdAsync(int rentalId)
        {
            IsValid.IsValidId(rentalId);

            var rental = await _context.Rentals
                .Include(r => r.User)
                .Include(r => r.Book)
                .SingleOrDefaultAsync(r => r.Id == rentalId && r.Status);

            if (rental is null)
            {
                throw new LibraryManagementSystemException("Kirayə yoxdur.");
            }
            else
            {
                return new RentalDTO
                {
                    RentalId = rental.Id,
                    BookTitle = rental.Book?.BookTitle,
                    UserName = rental.User?.Username,
                    RentalDate = rental.RentalDate,
                    DueDate = rental.DueDate,
                };
            }

        }
    }
}
