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
    public static class AreaEndpoints
    {
        public static IEndpointRouteBuilder MapAreas(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/areas/");
            group.MapGet("", GetAreas);
            group.MapGet("{id:int}", GetAreaById);
            group.MapPost("", CreateArea);
            group.MapPut("", UpdateArea);
            group.MapDelete("{id:int}", DeleteAreaById);
            return app;
        }

        public static async Task<Ok<IEnumerable<Area>>> GetAreas(IAreaService service)
        {
            var areas = await service.GetAreasAsync();
            return TypedResults.Ok(areas);
        }

        public static async Task<Results<NotFound, Ok<Area>>> GetAreaById(IAreaService service, int id)
        {
            var area = await service.GetAreaByIdAsync(id);
            if (area == null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(area);
        }

        public static async Task<Results<BadRequest<string>, Ok<Area>>> CreateArea(IAreaService service, Area area)
        {
            if (area == null)
            {
                return TypedResults.BadRequest("Invalid payload");
            }
            var result = await service.CreateAreaAsync(area);
            if (!result.IsSuccess)
            {
                return TypedResults.BadRequest(result.ErrorMessage!);
            }
            return TypedResults.Ok(result.Data!);
        }

        public static async Task<Results<NotFound, BadRequest, Ok>> UpdateArea(IAreaService service, Area area)
        {
            var existingArea = await service.GetAreaByIdAsync(area.IdArea);
            if (existingArea == null)
            {
                return TypedResults.NotFound();
            }
            var success = await service.UpdateAreaAsync(area);
            if (!success)
            {
                return TypedResults.BadRequest();
            }
            return TypedResults.Ok();
        }

        public static async Task<Results<NotFound, BadRequest, Ok>> DeleteAreaById(IAreaService service, int id)
        {
            var area = await service.GetAreaByIdAsync(id);
            if (area == null)
            {
                return TypedResults.NotFound();
            }
            var success = await service.DeleteAreaAsync(id);
            if (!success)
            {
                return TypedResults.BadRequest();
            }
            return TypedResults.Ok();
        }
    }
}