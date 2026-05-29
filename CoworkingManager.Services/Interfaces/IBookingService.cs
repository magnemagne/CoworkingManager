using System.Collections.Generic;
using System.Threading.Tasks;
using CoworkingManager.Models;

namespace CoworkingManager.Services.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetBookingsAsync();
        Task<Booking?> GetBookingByIdAsync(int id);
        Task<InsertResult<Booking>> CreateBookingAsync(Booking booking);
        Task<bool> UpdateBookingAsync(Booking booking);
        Task<bool> DeleteBookingAsync(int id);
    }
}