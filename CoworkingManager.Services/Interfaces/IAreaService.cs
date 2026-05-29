using System.Collections.Generic;
using System.Threading.Tasks;
using CoworkingManager.Models;

namespace CoworkingManager.Services.Interfaces
{
    public interface IAreaService
    {
        Task<IEnumerable<Area>> GetAreasAsync();
        Task<Area?> GetAreaByIdAsync(int id);
        Task<InsertResult<Area>> CreateAreaAsync(Area area);
        Task<bool> UpdateAreaAsync(Area area);
        Task<bool> DeleteAreaAsync(int id);
    }
}