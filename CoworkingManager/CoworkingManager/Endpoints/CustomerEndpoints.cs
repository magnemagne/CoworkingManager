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
    public static class CustomerEndpoints
    {
        public static IEndpointRouteBuilder MapCustomers(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/customers/");
            group.MapGet("", GetCustomers);
            group.MapGet("{id:int}", GetCustomerById);
            group.MapPost("", CreateCustomer);
            group.MapPut("", UpdateCustomer);
            group.MapDelete("{id:int}", DeleteCustomerById);
            return app;
        }

        public static async Task<Ok<IEnumerable<Customer>>> GetCustomers(ICustomerService service)
        {
            var customers = await service.GetCustomersAsync();
            return TypedResults.Ok(customers);
        }

        public static async Task<Results<NotFound, Ok<Customer>>> GetCustomerById(ICustomerService service, int id)
        {
            var customer = await service.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(customer);
        }

        public static async Task<Results<BadRequest<string>, Ok<Customer>>> CreateCustomer(ICustomerService service, Customer customer)
        {
            if (customer == null)
            {
                return TypedResults.BadRequest("Invalid payload");
            }
            var result = await service.CreateCustomerAsync(customer);
            if (!result.IsSuccess)
            {
                return TypedResults.BadRequest(result.ErrorMessage!);
            }
            return TypedResults.Ok(result.Data!);
        }

        public static async Task<Results<NotFound, BadRequest, Ok>> UpdateCustomer(ICustomerService service, Customer customer)
        {
            var existingCustomer = await service.GetCustomerByIdAsync(customer.Id);
            if (existingCustomer == null)
            {
                return TypedResults.NotFound();
            }
            var success = await service.UpdateCustomerAsync(customer);
            if (!success)
            {
                return TypedResults.BadRequest();
            }
            return TypedResults.Ok();
        }

        public static async Task<Results<NotFound, BadRequest, Ok>> DeleteCustomerById(ICustomerService service, int id)
        {
            var customer = await service.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return TypedResults.NotFound();
            }
            var success = await service.DeleteCustomerAsync(id);
            if (!success)
            {
                return TypedResults.BadRequest();
            }
            return TypedResults.Ok();
        }
    }
}