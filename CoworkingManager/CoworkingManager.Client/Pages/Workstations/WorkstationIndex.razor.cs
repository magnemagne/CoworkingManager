using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoworkingManager.Models;
using CoworkingManager.Client.Services;
using Microsoft.AspNetCore.Components;

namespace CoworkingManager.Client.Pages.Workstations
{
    public partial class WorkstationIndex : ComponentBase
    {
        [Inject] public WorkstationProxyService WorkstationService { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;

        private IEnumerable<Workstation>? workstations;

        protected override async Task OnInitializedAsync()
        {
            await LoadWorkstations();
        }

        private async Task LoadWorkstations()
        {
            workstations = await WorkstationService.GetWorkstationsAsync();
        }

        private async Task DeleteWorkstation(int id)
        {
            var success = await WorkstationService.DeleteWorkstationAsync(id);
            if (success)
            {
                await LoadWorkstations();
            }
        }
    }
}