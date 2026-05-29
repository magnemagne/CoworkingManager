using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CoworkingManager.Models;

namespace CoworkingManager.Client.Services
{
    public class WorkstationProxyService
    {
        private readonly HttpClient _httpClient;

        public WorkstationProxyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Workstation>?> GetWorkstationsAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Workstation>>("api/workstations");
        }

        public async Task<Workstation?> GetWorkstationByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Workstation>($"api/workstations/{id}");
        }

        public async Task<bool> IsWorkstationAvailableAsync(int id, DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            var url = $"api/workstations/{id}/available?date={date:yyyy-MM-dd}&startTime={startTime}&endTime={endTime}";
            return await _httpClient.GetFromJsonAsync<bool>(url);
        }

        public async Task<InsertResult<Workstation>> CreateWorkstationAsync(Workstation workstation)
        {
            var response = await _httpClient.PostAsJsonAsync("api/workstations", workstation);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<Workstation>();
                return new InsertResult<Workstation> { Data = data };
            }

            var error = await response.Content.ReadAsStringAsync();
            return new InsertResult<Workstation> { ErrorMessage = error };
        }

        public async Task<bool> UpdateWorkstationAsync(Workstation workstation)
        {
            var response = await _httpClient.PutAsJsonAsync("api/workstations", workstation);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteWorkstationAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/workstations/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}