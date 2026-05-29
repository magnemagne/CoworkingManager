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
    public class FeatureService : IFeatureService
    {
        private string _connectionString;
        private ILogger<FeatureService> _logger;

        public FeatureService(IConfiguration configuration,
                              ILogger<FeatureService> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new Exception("ConnectionString 'DefaultConnection' not found.");
            _logger = logger;
        }

        public async Task<IEnumerable<Feature>> GetFeaturesAsync()
        {
            using var connection = new MySqlConnection(_connectionString);
            const string query = """
                SELECT 
                idFeatures, 
                Name, 
                Description 
                FROM Features;
            """;
            return await connection.QueryAsync<Feature>(query);
        }

        public async Task<Feature?> GetFeatureByIdAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string query = """
                SELECT 
                idFeatures, 
                Name, 
                Description 
                FROM Features 
                WHERE idFeatures = @Id;
            """;
            return await connection.QueryFirstOrDefaultAsync<Feature>(query, new { Id = id });
        }

        public async Task<InsertResult<Feature>> CreateFeatureAsync(Feature feature)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                const string query = """
                        INSERT INTO 
                        Features (
                        Name, 
                        Description) 
                        VALUES (
                        @Name, 
                        @Description);
                        SELECT last_insert_id();
                    """;
                feature.IdFeatures = await connection.ExecuteScalarAsync<int>(query, feature);
                return new InsertResult<Feature> { Data = feature };
            }
            catch (MySqlException ex)
            {
                return new InsertResult<Feature> { ErrorMessage = ex.Message };
            }
        }

        public async Task<bool> UpdateFeatureAsync(Feature feature)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string query = """
                UPDATE Features 
                SET Name = @Name, 
                    Description = @Description 
                WHERE idFeatures = @IdFeatures;
            """;
            var rowsAffected = await connection.ExecuteAsync(query, feature);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteFeatureAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string query = """
                DELETE FROM Features 
                WHERE idFeatures = @Id;
            """;
            var rowsAffected = await connection.ExecuteAsync(query, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<bool> AssignFeatureToWorkstationAsync(int featureId, int workstationId)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string query = """
                INSERT INTO 
                Features_has_Workstation (
                Features_idFeatures, 
                Workstation_Id) 
                VALUES (
                @FeatureId, 
                @WorkstationId);
            """;
            var rowsAffected = await connection.ExecuteAsync(query, new { FeatureId = featureId, WorkstationId = workstationId });
            return rowsAffected > 0;
        }

        public async Task<bool> RemoveFeatureFromWorkstationAsync(int featureId, int workstationId)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string query = """
                DELETE FROM Features_has_Workstation 
                WHERE Features_idFeatures = @FeatureId 
                AND Workstation_Id = @WorkstationId;
            """;
            var rowsAffected = await connection.ExecuteAsync(query, new { FeatureId = featureId, WorkstationId = workstationId });
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Feature>> GetFeaturesByWorkstationIdAsync(int workstationId)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string query = """
                SELECT 
                f.idFeatures, 
                f.Name, 
                f.Description 
                FROM Features f
                INNER JOIN Features_has_Workstation fhw ON f.idFeatures = fhw.Features_idFeatures
                WHERE fhw.Workstation_Id = @WorkstationId;
            """;
            return await connection.QueryAsync<Feature>(query, new { WorkstationId = workstationId });
        }
    }
}