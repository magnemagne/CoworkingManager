using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CoworkingManager.Models;

namespace CoworkingManager.Client.Services
{
    public class CustomerProxyService
    {
        private readonly HttpClient _httpClient;

        public CustomerProxyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Customer>?> GetCustomersAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Customer>>("api/customers");
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Customer>($"api/customers/{id}");
        }

        public async Task<InsertResult<Customer>> CreateCustomerAsync(Customer customer)
        {
            var response = await _httpClient.PostAsJsonAsync("api/customers", customer);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<Customer>();
                return new InsertResult<Customer> { Data = data };
            }

            var error = await response.Content.ReadAsStringAsync();
            return new InsertResult<Customer> { ErrorMessage = error };
        }

        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            var response = await _httpClient.PutAsJsonAsync("api/customers", customer);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/customers/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}