using System;
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
    public static class WorkstationEndpoints
    {
        public static IEndpointRouteBuilder MapWorkstations(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/workstations/");
            group.MapGet("", GetWorkstations);
            group.MapGet("{id:int}", GetWorkstationById);
            group.MapGet("{id:int}/available", IsWorkstationAvailable);
            group.MapPost("", CreateWorkstation);
            group.MapPut("", UpdateWorkstation);
            group.MapDelete("{id:int}", DeleteWorkstationById);
            return app;
        }

        public static async Task<Ok<IEnumerable<Workstation>>> GetWorkstations(IWorkstationService service)
        {
            var workstations = await service.GetWorkstationsAsync();
            return TypedResults.Ok(workstations);
        }

        public static async Task<Results<NotFound, Ok<Workstation>>> GetWorkstationById(IWorkstationService service, int id)
        {
            var workstation = await service.GetWorkstationByIdAsync(id);
            if (workstation == null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(workstation);
        }

        public static async Task<Ok<bool>> IsWorkstationAvailable(IWorkstationService service, int id, DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            var isAvailable = await service.IsWorkstationAvailableAsync(id, date, startTime, endTime);
            return TypedResults.Ok(isAvailable);
        }

        public static async Task<Results<BadRequest<string>, Ok<Workstation>>> CreateWorkstation(IWorkstationService service, Workstation workstation)
        {
            if (workstation == null)
            {
                return TypedResults.BadRequest("Invalid payload");
            }
            var result = await service.CreateWorkstationAsync(workstation);
            if (!result.IsSuccess)
            {
                return TypedResults.BadRequest(result.ErrorMessage!);
            }
            return TypedResults.Ok(result.Data!);
        }

        public static async Task<Results<NotFound, BadRequest, Ok>> UpdateWorkstation(IWorkstationService service, Workstation workstation)
        {
            var existingWorkstation = await service.GetWorkstationByIdAsync(workstation.Id);
            if (existingWorkstation == null)
            {
                return TypedResults.NotFound();
            }
            var success = await service.UpdateWorkstationAsync(workstation);
            if (!success)
            {
                return TypedResults.BadRequest();
            }
            return TypedResults.Ok();
        }

        public static async Task<Results<NotFound, BadRequest, Ok>> DeleteWorkstationById(IWorkstationService service, int id)
        {
            var workstation = await service.GetWorkstationByIdAsync(id);
            if (workstation == null)
            {
                return TypedResults.NotFound();
            }
            var success = await service.DeleteWorkstationAsync(id);
            if (!success)
            {
                return TypedResults.BadRequest();
            }
            return TypedResults.Ok();
        }
    }
}