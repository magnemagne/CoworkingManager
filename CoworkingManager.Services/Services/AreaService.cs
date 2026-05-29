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
    public class AreaService : IAreaService
    {
        private string _connectionString;
        private ILogger<AreaService> _logger;

        public AreaService(IConfiguration configuration,
                           ILogger<AreaService> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new Exception("ConnectionString 'DefaultConnection' not found.");
            _logger = logger;
        }

        public async Task<IEnumerable<Area>> GetAreasAsync()
        {
            using var connection = new MySqlConnection(_connectionString);
            const string query = """
                SELECT 
                idArea, 
                Name, 
                Info 
                FROM Area;
            """;
            return await connection.QueryAsync<Area>(query);
        }

        public async Task<Area?> GetAreaByIdAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string query = """
                SELECT 
                idArea, 
                Name, 
                Info 
                FROM Area 
                WHERE idArea = @Id;
            """;
            return await connection.QueryFirstOrDefaultAsync<Area>(query, new { Id = id });
        }

        public async Task<InsertResult<Area>> CreateAreaAsync(Area area)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                const string query = """
                        INSERT INTO 
                        Area (
                        Name, 
                        Info) 
                        VALUES (
                        @Name, 
                        @Info);
                        SELECT last_insert_id();
                    """;
                area.IdArea = await connection.ExecuteScalarAsync<int>(query, area);
                return new InsertResult<Area> { Data = area };
            }
            catch (MySqlException ex)
            {
                return new InsertResult<Area> { ErrorMessage = ex.Message };
            }
        }

        public async Task<bool> UpdateAreaAsync(Area area)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string query = """
                UPDATE Area 
                SET Name = @Name, 
                    Info = @Info 
                WHERE idArea = @IdArea;
            """;
            var rowsAffected = await connection.ExecuteAsync(query, area);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAreaAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string query = """
                DELETE FROM Area 
                WHERE idArea = @Id;
            """;
            var rowsAffected = await connection.ExecuteAsync(query, new { Id = id });
            return rowsAffected > 0;
        }
    }
}