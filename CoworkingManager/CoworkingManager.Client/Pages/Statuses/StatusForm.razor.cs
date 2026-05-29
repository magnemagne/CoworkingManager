using System.Collections.Generic;
using System.Threading.Tasks;
using CoworkingManager.Models;
using CoworkingManager.Client.Services;
using Microsoft.AspNetCore.Components;

namespace CoworkingManager.Client.Pages.Statuses
{
    public partial class StatusForm : ComponentBase
    {
        [Inject] public StatusProxyService StatusService { get; set; } = default!;
        [Inject] public BookingProxyService BookingService { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;

        [Parameter] public int? Id { get; set; }

        private Status? statusItem;
        private IEnumerable<Booking>? bookings;
        private string? errorMessage;
        private bool IsEditMode => Id.HasValue;

        protected override async Task OnInitializedAsync()
        {
            bookings = await BookingService.GetBookingsAsync() ?? new List<Booking>();

            if (IsEditMode)
            {
                statusItem = await StatusService.GetStatusByIdAsync(Id!.Value);
                if (statusItem == null)
                {
                    NavigationManager.NavigateTo("/not-found");
                }
            }
            else
            {
                statusItem = new Status();
            }
        }

        private async Task HandleSubmit()
        {
            errorMessage = null;

            if (statusItem!.Booking.Id == 0)
            {
                errorMessage = "Please assign this status to a valid Booking.";
                return;
            }

            if (IsEditMode)
            {
                var success = await StatusService.UpdateStatusAsync(statusItem);
                if (success) NavigationManager.NavigateTo("/statuses");
                else errorMessage = "An error occurred while updating the status.";
            }
            else
            {
                var result = await StatusService.CreateStatusAsync(statusItem);
                if (result.IsSuccess) NavigationManager.NavigateTo("/statuses");
                else errorMessage = result.ErrorMessage;
            }
        }
    }
}