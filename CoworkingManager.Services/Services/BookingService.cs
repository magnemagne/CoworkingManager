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
    public class BookingService : IBookingService
    {
        private string _connectionString;
        private ILogger<BookingService> _logger;
        private IWorkstationService _workstationService;

        public BookingService(IConfiguration configuration,
                              ILogger<BookingService> logger,
                              IWorkstationService workstationService)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new Exception("ConnectionString 'DefaultConnection' not found.");
            _logger = logger;
            _workstationService = workstationService;
        }

        public async Task<IEnumerable<Booking>> GetBookingsAsync()
        {
            using var connection = new MySqlConnection(_connectionString);
            const string query = """
                SELECT 
                b.Id, 
                b.DateStart, 
                b.DateEnd, 
                b.LastUpdate, 
                b.Notes,
                c.Id, 
                c.Name, 
                c.Address, 
                c.Email,
                w.Id, 
                w.Description, 
                w.Opening, 
                w.Closing, 
                w.MaxReservations
                FROM ` Booking` b
                INNER JOIN Customer c ON b.idClient = c.Id
                INNER JOIN Workstation w ON b.idWorkstation = w.Id;
            """;

            return await connection.QueryAsync<Booking, Customer, Workstation, Booking>(query, (booking, customer, workstation) =>
            {
                booking.Customer = customer;
                booking.Workstation = workstation;
                return booking;
            }, splitOn: "Id,Id");
        }

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string query = """
                SELECT 
                b.Id, 
                b.DateStart, 
                b.DateEnd, 
                b.LastUpdate, 
                b.Notes,
                c.Id, 
                c.Name, 
                c.Address, 
                c.Email,
                w.Id, 
                w.Description, 
                w.Opening, 
                w.Closing, 
                w.MaxReservations
                FROM ` Booking` b
                INNER JOIN Customer c ON b.idClient = c.Id
                INNER JOIN Workstation w ON b.idWorkstation = w.Id
                WHERE b.Id = @Id;
            """;

            var result = await connection.QueryAsync<Booking, Customer, Workstation, Booking>(query, (booking, customer, workstation) =>
            {
                booking.Customer = customer;
                booking.Workstation = workstation;
                return booking;
            }, new { Id = id }, splitOn: "Id,Id");

            return result.AsList().Count > 0 ? result.AsList()[0] : null;
        }

        public async Task<InsertResult<Booking>> CreateBookingAsync(Booking booking)
        {
            try
            {
                var date = booking.DateStart.Value.Date;
                var startTime = booking.DateStart.Value.TimeOfDay;
                var endTime = booking.DateEnd.Value.TimeOfDay;

                bool isAvailable = await _workstationService.IsWorkstationAvailableAsync(
                    booking.Workstation.Id, date, startTime, endTime);

                if (!isAvailable)
                {
                    return new InsertResult<Booking> { ErrorMessage = "The selected workstation is already booked during this time." }; ;
                }

                using var connection = new MySqlConnection(_connectionString);
                const string query = """
                        INSERT INTO 
                        ` Booking` (
                        DateStart, 
                        DateEnd, 
                        idClient, 
                        idWorkstation, 
                        Notes) 
                        VALUES (
                        @DateStart, 
                        @DateEnd, 
                        @IdClient, 
                        @IdWorkstation, 
                        @Notes);
                        SELECT last_insert_id();
                    """;

                var parameters = new
                {
                    booking.DateStart,
                    booking.DateEnd,
                    IdClient = booking.Customer?.Id ?? 0,
                    IdWorkstation = booking.Workstation?.Id ?? 0,
                    booking.Notes
                };

                booking.Id = await connection.ExecuteScalarAsync<int>(query, parameters);
                return new InsertResult<Booking> { Data = booking };
            }
            catch (MySqlException ex)
            {
                return new InsertResult<Booking> { ErrorMessage = ex.Message };
            }
        }

        public async Task<bool> UpdateBookingAsync(Booking booking)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string query = """
                UPDATE ` Booking` 
                SET DateStart = @DateStart, 
                    DateEnd = @DateEnd, 
                    LastUpdate = CURRENT_TIMESTAMP, 
                    idClient = @IdClient, 
                    idWorkstation = @IdWorkstation, 
                    Notes = @Notes 
                WHERE Id = @Id;
            """;

            var parameters = new
            {
                booking.Id,
                booking.DateStart,
                booking.DateEnd,
                IdClient = booking.Customer?.Id ?? 0,
                IdWorkstation = booking.Workstation?.Id ?? 0,
                booking.Notes
            };

            var rowsAffected = await connection.ExecuteAsync(query, parameters);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteBookingAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string query = """
                DELETE FROM ` Booking` 
                WHERE Id = @Id;
            """;
            var rowsAffected = await connection.ExecuteAsync(query, new { Id = id });
            return rowsAffected > 0;
        }
    }
}