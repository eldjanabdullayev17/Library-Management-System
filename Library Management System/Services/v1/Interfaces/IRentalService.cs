using Library_Management_System.DTOs.Rental;
using Library_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Services.v1.Interfaces
{
    public interface IRentalService
    {
        Task<AddRentalDTO> addRentalAsync(AddRentalDTO rental);

        Task<bool> updateRentalAsync(int rentalId, UpdateRentalDTO newRental);

        Task<RentalDTO> getRentalByIdAsync(int rentalId);

    }
}
