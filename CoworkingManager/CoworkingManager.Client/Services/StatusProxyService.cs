using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CoworkingManager.Models;

namespace CoworkingManager.Client.Services
{
    public class StatusProxyService
    {
        private readonly HttpClient _httpClient;

        public StatusProxyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Status>?> GetStatusesAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Status>>("api/statuses");
        }

        public async Task<Status?> GetStatusByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Status>($"api/statuses/{id}");
        }

        public async Task<IEnumerable<Status>?> GetStatusesByBookingIdAsync(int bookingId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Status>>($"api/statuses/booking/{bookingId}");
        }

        public async Task<InsertResult<Status>> CreateStatusAsync(Status status)
        {
            var response = await _httpClient.PostAsJsonAsync("api/statuses", status);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<Status>();
                return new InsertResult<Status> { Data = data };
            }

            var error = await response.Content.ReadAsStringAsync();
            return new InsertResult<Status> { ErrorMessage = error };
        }

        public async Task<bool> UpdateStatusAsync(Status status)
        {
            var response = await _httpClient.PutAsJsonAsync("api/statuses", status);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteStatusAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/statuses/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}