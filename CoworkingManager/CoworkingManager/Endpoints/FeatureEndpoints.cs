using CoworkingManager.Services.Interfaces;
using CoworkingManager.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoworkingManager.Backend.Endpoints
{
    public static class FeatureEndpoints
    {
        public static IEndpointRouteBuilder MapFeatures(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/features/");
            group.MapGet("", GetFeatures);
            group.MapGet("{id:int}", GetFeatureById);
            group.MapGet("workstation/{workstationId:int}", GetFeaturesByWorkstationId);
            group.MapPost("", CreateFeature);
            group.MapPost("{featureId:int}/workstation/{workstationId:int}", AssignFeatureToWorkstation);
            group.MapPut("", UpdateFeature);
            group.MapDelete("{id:int}", DeleteFeatureById);
            group.MapDelete("{featureId:int}/workstation/{workstationId:int}", RemoveFeatureFromWorkstation);
            return app;
        }

        public static async Task<Ok<IEnumerable<Feature>>> GetFeatures(IFeatureService service)
        {
            var features = await service.GetFeaturesAsync();
            return TypedResults.Ok(features);
        }

        public static async Task<Results<NotFound, Ok<Feature>>> GetFeatureById(IFeatureService service, int id)
        {
            var feature = await service.GetFeatureByIdAsync(id);
            if (feature == null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(feature);
        }

        public static async Task<Ok<IEnumerable<Feature>>> GetFeaturesByWorkstationId(IFeatureService service, int workstationId)
        {
            var features = await service.GetFeaturesByWorkstationIdAsync(workstationId);
            return TypedResults.Ok(features);
        }

        public static async Task<Results<BadRequest<string>, Ok<Feature>>> CreateFeature(IFeatureService service, Feature feature)
        {
            if (feature == null)
            {
                return TypedResults.BadRequest("Invalid payload");
            }
            var result = await service.CreateFeatureAsync(feature);
            if (!result.IsSuccess)
            {
                return TypedResults.BadRequest(result.ErrorMessage!);
            }
            return TypedResults.Ok(result.Data!);
        }

        public static async Task<Results<BadRequest, Ok>> AssignFeatureToWorkstation(IFeatureService service, int featureId, int workstationId)
        {
            var success = await service.AssignFeatureToWorkstationAsync(featureId, workstationId);
            if (!success)
            {
                return TypedResults.BadRequest();
            }
            return TypedResults.Ok();
        }

        public static async Task<Results<NotFound, BadRequest, Ok>> UpdateFeature(IFeatureService service, Feature feature)
        {
            var existingFeature = await service.GetFeatureByIdAsync(feature.IdFeatures);
            if (existingFeature == null)
            {
                return TypedResults.NotFound();
            }
            var success = await service.UpdateFeatureAsync(feature);
            if (!success)
            {
                return TypedResults.BadRequest();
            }
            return TypedResults.Ok();
        }

        public static async Task<Results<NotFound, BadRequest, Ok>> DeleteFeatureById(IFeatureService service, int id)
        {
            var feature = await service.GetFeatureByIdAsync(id);
            if (feature == null)
            {
                return TypedResults.NotFound();
            }
            var success = await service.DeleteFeatureAsync(id);
            if (!success)
            {
                return TypedResults.BadRequest();
            }
            return TypedResults.Ok();
        }

        public static async Task<Results<BadRequest, Ok>> RemoveFeatureFromWorkstation(IFeatureService service, int featureId, int workstationId)
        {
            var success = await service.RemoveFeatureFromWorkstationAsync(featureId, workstationId);
            if (!success)
            {
                return TypedResults.BadRequest();
            }
            return TypedResults.Ok();
        }
    }
}