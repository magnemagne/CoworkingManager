using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CoworkingManager.Models;

namespace CoworkingManager.Client.Services
{
    public class BookingProxyService
    {
        private readonly HttpClient _httpClient;

        public BookingProxyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Booking>?> GetBookingsAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Booking>>("api/bookings");
        }

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Booking>($"api/bookings/{id}");
        }

        public async Task<InsertResult<Booking>> CreateBookingAsync(Booking booking)
        {
            var response = await _httpClient.PostAsJsonAsync("api/bookings", booking);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<Booking>();
                return new InsertResult<Booking> { Data = data };
            }

            var error = await response.Content.ReadAsStringAsync();
            return new InsertResult<Booking> { ErrorMessage = error };
        }

        public async Task<bool> UpdateBookingAsync(Booking booking)
        {
            var response = await _httpClient.PutAsJsonAsync("api/bookings", booking);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteBookingAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/bookings/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}