using CoworkingManager.Models;
using CoworkingManager.Services.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoworkingManager.Services.Services
{
    public class CustomerService : ICustomerService
    {
        private string _connectionString;
        private ILogger<CustomerService> _logger;

        public CustomerService(IConfiguration configuration,
                               ILogger<CustomerService> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new Exception("ConnectionString 'DefaultConnection' not found.");
            _logger = logger;
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            using var connection = new MySqlConnection(_connectionString);
            const string query = """
                SELECT 
                Id, 
                Name, 
                Address, 
                Email 
                FROM Customer;
            """;
            return await connection.QueryAsync<Customer>(query);
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string query = """
                SELECT 
                Id, 
                Name, 
                Address, 
                Email 
                FROM Customer 
                WHERE Id = @Id;
            """;
            return await connection.QueryFirstOrDefaultAsync<Customer>(query, new { Id = id });
        }

        public async Task<InsertResult<Customer>> CreateCustomerAsync(Customer customer)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                const string query = """
                        INSERT INTO 
                        Customer (
                        Name, 
                        Address, 
                        Email) 
                        VALUES (
                        @Name, 
                        @Address, 
                        @Email);
                        SELECT last_insert_id();
                    """;
                customer.Id = await connection.ExecuteScalarAsync<int>(query, customer);
                return new InsertResult<Customer> { Data = customer };
            }
            catch (MySqlException ex)
            {
                return new InsertResult<Customer> { ErrorMessage = ex.Message };
            }
        }

        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string query = """
                UPDATE Customer 
                SET Name = @Name, 
                    Address = @Address, 
                    Email = @Email 
                WHERE Id = @Id;
            """;
            var rowsAffected = await connection.ExecuteAsync(query, customer);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string query = """
                DELETE FROM Customer 
                WHERE Id = @Id;
            """;
            var rowsAffected = await connection.ExecuteAsync(query, new { Id = id });
            return rowsAffected > 0;
        }
    }
}