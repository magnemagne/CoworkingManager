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
    public class StatusService : IStatusService
    {
        private string _connectionString;
        private ILogger<StatusService> _logger;

        public StatusService(IConfiguration configuration,
                             ILogger<StatusService> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new Exception("ConnectionString 'DefaultConnection' not found.");
            _logger = logger;
        }

        public async Task<IEnumerable<Status>> GetStatusesAsync()
        {
            using var connection = new MySqlConnection(_connectionString);
            const string query = """
                SELECT 
                s.Id, 
                s.Status AS StatusValue, 
                b.Id, 
                b.DateStart, 
                b.DateEnd, 
                b.LastUpdate, 
                b.Notes
                FROM Status s
                INNER JOIN ` Booking` b ON s.idBooking = b.Id;
            """;
            return await connection.QueryAsync<Status, Booking, Status>(query, (status, booking) =>
            {
                status.Booking = booking;
                return status;
            }, splitOn: "Id");
        }

        public async Task<Status?> GetStatusByIdAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string query = """
                SELECT 
                s.Id, 
                s.Status AS StatusValue, 
                b.Id, 
                b.DateStart, 
                b.DateEnd, 
                b.LastUpdate, 
                b.Notes
                FROM Status s
                INNER JOIN ` Booking` b ON s.idBooking = b.Id
                WHERE s.Id = @Id;
            """;
            var result = await connection.QueryAsync<Status, Booking, Status>(query, (status, booking) =>
            {
                status.Booking = booking;
                return status;
            }, new { Id = id }, splitOn: "Id");
            return result.AsList().Count > 0 ? result.AsList()[0] : null;
        }

        public async Task<InsertResult<Status>> CreateStatusAsync(Status status)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                const string query = """
                        INSERT INTO 
                        Status (
                        Status, 
                        idBooking) 
                        VALUES (
                        @StatusValue, 
                        @IdBooking);
                        SELECT last_insert_id();
                    """;
                var parameters = new
                {
                    status.StatusValue,
                    IdBooking = status.Booking?.Id ?? 0
                };
                status.Id = await connection.ExecuteScalarAsync<int>(query, parameters);
                return new InsertResult<Status> { Data = status };
            }
            catch (MySqlException ex)
            {
                return new InsertResult<Status> { ErrorMessage = ex.Message };
            }
        }

        public async Task<bool> UpdateStatusAsync(Status status)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string query = """
                UPDATE Status 
                SET Status = @StatusValue, 
                    idBooking = @IdBooking 
                WHERE Id = @Id;
            """;
            var parameters = new
            {
                status.Id,
                status.StatusValue,
                IdBooking = status.Booking?.Id ?? 0
            };
            var rowsAffected = await connection.ExecuteAsync(query, parameters);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteStatusAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string query = """
                DELETE FROM Status 
                WHERE Id = @Id;
            """;
            var rowsAffected = await connection.ExecuteAsync(query, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Status>> GetStatusesByBookingIdAsync(int bookingId)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string query = """
                SELECT 
                Id, 
                Status AS StatusValue 
                FROM Status 
                WHERE idBooking = @BookingId;
            """;
            return await connection.QueryAsync<Status>(query, new { BookingId = bookingId });
        }
    }
}