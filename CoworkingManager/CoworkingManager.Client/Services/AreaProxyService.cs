using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CoworkingManager.Models;

namespace CoworkingManager.Client.Services
{
    public class AreaProxyService
    {
        private readonly HttpClient _httpClient;

        public AreaProxyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Area>?> GetAreasAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Area>>("api/areas");
        }

        public async Task<Area?> GetAreaByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Area>($"api/areas/{id}");
        }

        public async Task<InsertResult<Area>> CreateAreaAsync(Area area)
        {
            var response = await _httpClient.PostAsJsonAsync("api/areas", area);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<Area>();
                return new InsertResult<Area> { Data = data };
            }

            var error = await response.Content.ReadAsStringAsync();
            return new InsertResult<Area> { ErrorMessage = error };
        }

        public async Task<bool> UpdateAreaAsync(Area area)
        {
            var response = await _httpClient.PutAsJsonAsync("api/areas", area);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAreaAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/areas/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}