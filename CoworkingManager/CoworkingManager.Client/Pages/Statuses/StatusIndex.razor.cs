using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoworkingManager.Models;
using CoworkingManager.Client.Services;
using Microsoft.AspNetCore.Components;

namespace CoworkingManager.Client.Pages.Statuses
{
    public partial class StatusIndex : ComponentBase
    {
        [Inject] public StatusProxyService StatusService { get; set; } = default!;

        private IEnumerable<Status>? statuses;

        protected override async Task OnInitializedAsync()
        {
            await LoadStatuses();
        }

        private async Task LoadStatuses()
        {
            statuses = await StatusService.GetStatusesAsync();
        }

        private async Task DeleteStatus(int id)
        {
            var success = await StatusService.DeleteStatusAsync(id);
            if (success)
            {
                await LoadStatuses();
            }
        }
    }
}