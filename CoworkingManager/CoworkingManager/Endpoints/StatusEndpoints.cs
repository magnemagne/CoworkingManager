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
    public static class StatusEndpoints
    {
        public static IEndpointRouteBuilder MapStatuses(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/statuses/");
            group.MapGet("", GetStatuses);
            group.MapGet("{id:int}", GetStatusById);
            group.MapGet("booking/{bookingId:int}", GetStatusesByBookingId);
            group.MapPost("", CreateStatus);
            group.MapPut("", UpdateStatus);
            group.MapDelete("{id:int}", DeleteStatusById);
            return app;
        }

        public static async Task<Ok<IEnumerable<Status>>> GetStatuses(IStatusService service)
        {
            var statuses = await service.GetStatusesAsync();
            return TypedResults.Ok(statuses);
        }

        public static async Task<Results<NotFound, Ok<Status>>> GetStatusById(IStatusService service, int id)
        {
            var status = await service.GetStatusByIdAsync(id);
            if (status == null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(status);
        }

        public static async Task<Ok<IEnumerable<Status>>> GetStatusesByBookingId(IStatusService service, int bookingId)
        {
            var statuses = await service.GetStatusesByBookingIdAsync(bookingId);
            return TypedResults.Ok(statuses);
        }

        public static async Task<Results<BadRequest<string>, Ok<Status>>> CreateStatus(IStatusService service, Status status)
        {
            if (status == null)
            {
                return TypedResults.BadRequest("Invalid payload");
            }
            var result = await service.CreateStatusAsync(status);
            if (!result.IsSuccess)
            {
                return TypedResults.BadRequest(result.ErrorMessage!);
            }
            return TypedResults.Ok(result.Data!);
        }

        public static async Task<Results<NotFound, BadRequest, Ok>> UpdateStatus(IStatusService service, Status status)
        {
            var existingStatus = await service.GetStatusByIdAsync(status.Id);
            if (existingStatus == null)
            {
                return TypedResults.NotFound();
            }
            var success = await service.UpdateStatusAsync(status);
            if (!success)
            {
                return TypedResults.BadRequest();
            }
            return TypedResults.Ok();
        }

        public static async Task<Results<NotFound, BadRequest, Ok>> DeleteStatusById(IStatusService service, int id)
        {
            var status = await service.GetStatusByIdAsync(id);
            if (status == null)
            {
                return TypedResults.NotFound();
            }
            var success = await service.DeleteStatusAsync(id);
            if (!success)
            {
                return TypedResults.BadRequest();
            }
            return TypedResults.Ok();
        }
    }
}