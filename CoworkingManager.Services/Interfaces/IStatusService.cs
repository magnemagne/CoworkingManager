using System.Collections.Generic;
using System.Threading.Tasks;
using CoworkingManager.Models;

namespace CoworkingManager.Services.Interfaces
{
    public interface IStatusService
    {
        Task<IEnumerable<Status>> GetStatusesAsync();
        Task<Status?> GetStatusByIdAsync(int id);
        Task<InsertResult<Status>> CreateStatusAsync(Status status);
        Task<bool> UpdateStatusAsync(Status status);
        Task<bool> DeleteStatusAsync(int id);
        Task<IEnumerable<Status>> GetStatusesByBookingIdAsync(int bookingId);
    }
}