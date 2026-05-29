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
    public class WorkstationService : IWorkstationService
    {
        private string _connectionString;
        private ILogger<WorkstationService> _logger;

        public WorkstationService(IConfiguration configuration,
                                  ILogger<WorkstationService> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new Exception("ConnectionString 'DefaultConnection' not found.");
            _logger = logger;
        }

        public async Task<IEnumerable<Workstation>> GetWorkstationsAsync()
        {
            using var connection = new MySqlConnection(_connectionString);
            const string query = """
                SELECT 
                w.Id, 
                w.Description, 
                w.Opening, 
                w.Closing, 
                w.MaxReservations, 
                a.idArea, 
                a.Name, 
                a.Info
                FROM Workstation w
                INNER JOIN Area a ON w.idArea = a.idArea;
            """;
            return await connection.QueryAsync<Workstation, Area, Workstation>(query, (workstation, area) =>
            {
                workstation.Area = area;
                return workstation;
            }, splitOn: "idArea");
        }

        public async Task<Workstation?> GetWorkstationByIdAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string query = """
                SELECT 
                w.Id, 
                w.Description, 
                w.Opening, 
                w.Closing, 
                w.MaxReservations, 
                a.idArea, 
                a.Name, 
                a.Info
                FROM Workstation w
                INNER JOIN Area a ON w.idArea = a.idArea
                WHERE w.Id = @Id;
            """;
            var result = await connection.QueryAsync<Workstation, Area, Workstation>(query, (workstation, area) =>
            {
                workstation.Area = area;
                return workstation;
            }, new { Id = id }, splitOn: "idArea");
            return result.AsList().Count > 0 ? result.AsList()[0] : null;
        }

        public async Task<InsertResult<Workstation>> CreateWorkstationAsync(Workstation workstation)
        {
            var result = new InsertResult<Workstation>();

            try
            {
                using var connection = new MySqlConnection(_connectionString);
                const string query = """
                    INSERT INTO 
                    Workstation (
                    Description, 
                    Opening, 
                    Closing, 
                    MaxReservations, 
                    idArea) 
                    VALUES (
                    @Description, 
                    @Opening, 
                    @Closing, 
                    @MaxReservations, 
                    @IdArea);
                    SELECT last_insert_id();
                """;

                var parameters = new
                {
                    workstation.Description,
                    workstation.Opening,
                    workstation.Closing,
                    workstation.MaxReservations,
                    IdArea = workstation.Area?.IdArea ?? 0
                };

                workstation.Id = await connection.ExecuteScalarAsync<int>(query, parameters);
                result.Data = workstation;
            }
            catch (MySqlException ex)
            {
                _logger.LogError(ex, "Database error creating workstation.");
                result.ErrorMessage = $"A database error occurred: {ex.Message}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating workstation.");
                result.ErrorMessage = "An unexpected error occurred while saving the workstation.";
            }

            return result;
        }

        public async Task<bool> IsWorkstationAvailableAsync(int workstationId, DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string query = """
                SELECT COUNT(*) 
                FROM ` Booking` 
                WHERE idWorkstation = @WorkstationId 
                AND DATE(DateStart) = DATE(@Date)
                AND (@StartTime < TIME(DateEnd) AND @EndTime > TIME(DateStart));
            """;
            var parameters = new
            {
                WorkstationId = workstationId,
                Date = date,
                StartTime = startTime,
                EndTime = endTime
            };
            var count = await connection.ExecuteScalarAsync<int>(query, parameters);
            return count == 0;
        }

        public async Task<bool> UpdateWorkstationAsync(Workstation workstation)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string query = """
                UPDATE Workstation 
                SET Description = @Description, 
                    Opening = @Opening, 
                    Closing = @Closing, 
                    MaxReservations = @MaxReservations, 
                    idArea = @IdArea 
                WHERE Id = @Id;
            """;
            var parameters = new
            {
                workstation.Id,
                workstation.Description,
                workstation.Opening,
                workstation.Closing,
                workstation.MaxReservations,
                IdArea = workstation.Area?.IdArea ?? 0
            };
            var rowsAffected = await connection.ExecuteAsync(query, parameters);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteWorkstationAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string query = """
                DELETE FROM Workstation 
                WHERE Id = @Id;
            """;
            var rowsAffected = await connection.ExecuteAsync(query, new { Id = id });
            return rowsAffected > 0;
        }
    }
}