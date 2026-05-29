using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoworkingManager.Models;
using CoworkingManager.Client.Services;
using Microsoft.AspNetCore.Components;

namespace CoworkingManager.Client.Pages.Bookings
{
    public partial class BookingIndex : ComponentBase
    {
        [Inject] public BookingProxyService BookingService { get; set; } = default!;

        private IEnumerable<Booking>? bookings;

        protected override async Task OnInitializedAsync()
        {
            await LoadBookings();
        }

        private async Task LoadBookings()
        {
            bookings = await BookingService.GetBookingsAsync();
        }

        private async Task DeleteBooking(int id)
        {
            var success = await BookingService.DeleteBookingAsync(id);
            if (success)
            {
                await LoadBookings();
            }
        }
    }
}