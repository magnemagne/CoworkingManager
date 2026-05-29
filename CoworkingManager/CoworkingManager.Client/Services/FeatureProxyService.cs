using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CoworkingManager.Models;

namespace CoworkingManager.Client.Services
{
    public class FeatureProxyService
    {
        private readonly HttpClient _httpClient;

        public FeatureProxyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Feature>?> GetFeaturesAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Feature>>("api/features");
        }

        public async Task<Feature?> GetFeatureByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Feature>($"api/features/{id}");
        }

        public async Task<IEnumerable<Feature>?> GetFeaturesByWorkstationIdAsync(int workstationId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Feature>>($"api/features/workstation/{workstationId}");
        }

        public async Task<InsertResult<Feature>> CreateFeatureAsync(Feature feature)
        {
            var response = await _httpClient.PostAsJsonAsync("api/features", feature);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<Feature>();
                return new InsertResult<Feature> { Data = data };
            }

            var error = await response.Content.ReadAsStringAsync();
            return new InsertResult<Feature> { ErrorMessage = error };
        }

        public async Task<bool> AssignFeatureToWorkstationAsync(int featureId, int workstationId)
        {
            var response = await _httpClient.PostAsync($"api/features/{featureId}/workstation/{workstationId}", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateFeatureAsync(Feature feature)
        {
            var response = await _httpClient.PutAsJsonAsync("api/features", feature);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteFeatureAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/features/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveFeatureFromWorkstationAsync(int featureId, int workstationId)
        {
            var response = await _httpClient.DeleteAsync($"api/features/{featureId}/workstation/{workstationId}");
            return response.IsSuccessStatusCode;
        }
    }
}