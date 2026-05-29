using System.Collections.Generic;
using System.Threading.Tasks;
using CoworkingManager.Models;

namespace CoworkingManager.Services.Interfaces
{
    public interface IFeatureService
    {
        Task<IEnumerable<Feature>> GetFeaturesAsync();
        Task<Feature?> GetFeatureByIdAsync(int id);
        Task<InsertResult<Feature>> CreateFeatureAsync(Feature feature);
        Task<bool> UpdateFeatureAsync(Feature feature);
        Task<bool> DeleteFeatureAsync(int id);
        Task<bool> AssignFeatureToWorkstationAsync(int featureId, int workstationId);
        Task<bool> RemoveFeatureFromWorkstationAsync(int featureId, int workstationId);
        Task<IEnumerable<Feature>> GetFeaturesByWorkstationIdAsync(int workstationId);
    }
}