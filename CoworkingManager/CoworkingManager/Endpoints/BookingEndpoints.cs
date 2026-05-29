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
    public static class BookingEndpoints
    {
        public static IEndpointRouteBuilder MapBookings(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/bookings/");
            group.MapGet("", GetBookings);
            group.MapGet("{id:int}", GetBookingById);
            group.MapPost("", CreateBooking);
            group.MapPut("", UpdateBooking);
            group.MapDelete("{id:int}", DeleteBookingById);
            return app;
        }

        public static async Task<Ok<IEnumerable<Booking>>> GetBookings(IBookingService service)
        {
            var bookings = await service.GetBookingsAsync();
            return TypedResults.Ok(bookings);
        }

        public static async Task<Results<NotFound, Ok<Booking>>> GetBookingById(IBookingService service, int id)
        {
            var booking = await service.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(booking);
        }

        public static async Task<Results<BadRequest<string>, Ok<Booking>>> CreateBooking(IBookingService service, Booking booking)
        {
            if (booking == null)
            {
                return TypedResults.BadRequest("Invalid payload");
            }
            var result = await service.CreateBookingAsync(booking);
            if (!result.IsSuccess)
            {
                return TypedResults.BadRequest(result.ErrorMessage!);
            }
            return TypedResults.Ok(result.Data!);
        }

        public static async Task<Results<NotFound, BadRequest, Ok>> UpdateBooking(IBookingService service, Booking booking)
        {
            var existingBooking = await service.GetBookingByIdAsync(booking.Id);
            if (existingBooking == null)
            {
                return TypedResults.NotFound();
            }
            var success = await service.UpdateBookingAsync(booking);
            if (!success)
            {
                return TypedResults.BadRequest();
            }
            return TypedResults.Ok();
        }

        public static async Task<Results<NotFound, BadRequest, Ok>> DeleteBookingById(IBookingService service, int id)
        {
            var booking = await service.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return TypedResults.NotFound();
            }
            var success = await service.DeleteBookingAsync(id);
            if (!success)
            {
                return TypedResults.BadRequest();
            }
            return TypedResults.Ok();
        }
    }
}