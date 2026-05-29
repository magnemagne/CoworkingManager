using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoworkingManager.Models;
using CoworkingManager.Client.Services;
using Microsoft.AspNetCore.Components;

namespace CoworkingManager.Client.Pages.Bookings
{
    public partial class BookingForm : ComponentBase
    {
        [Inject] public BookingProxyService BookingService { get; set; } = default!;
        [Inject] public CustomerProxyService CustomerService { get; set; } = default!;
        [Inject] public WorkstationProxyService WorkstationService { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;

        [Parameter] public int? Id { get; set; }

        private Booking? booking;
        private IEnumerable<Customer>? customers;
        private IEnumerable<Workstation>? workstations;
        private string? errorMessage;
        private bool IsEditMode => Id.HasValue;

        protected override async Task OnInitializedAsync()
        {
            customers = await CustomerService.GetCustomersAsync() ?? new List<Customer>();
            workstations = await WorkstationService.GetWorkstationsAsync() ?? new List<Workstation>();

            if (IsEditMode)
            {
                booking = await BookingService.GetBookingByIdAsync(Id!.Value);
                if (booking == null)
                {
                    NavigationManager.NavigateTo("/not-found");
                }
            }
            else
            {
                booking = new Booking
                {
                    DateStart = DateTime.Now,
                    DateEnd = DateTime.Now.AddHours(1)
                };
            }
        }

        private async Task HandleSubmit()
        {
            errorMessage = null;

            if (booking!.Workstation.Id == 0 || booking.Workstation.Id == 0)
            {
                errorMessage = "Please select both a Customer and a Workstation.";
                return;
            }

            if (IsEditMode)
            {
                var success = await BookingService.UpdateBookingAsync(booking);
                if (success) NavigationManager.NavigateTo("/bookings");
                else errorMessage = "An error occurred while updating the booking.";
            }
            else
            {
                var result = await BookingService.CreateBookingAsync(booking);
                if (result.IsSuccess) NavigationManager.NavigateTo("/bookings");
                else errorMessage = result.ErrorMessage;
            }
        }
    }
}