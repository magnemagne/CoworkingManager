using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoworkingManager.Models;
using CoworkingManager.Client.Services;
using Microsoft.AspNetCore.Components;

namespace CoworkingManager.Client.Pages.Areas
{
    public partial class AreaIndex : ComponentBase
    {
        [Inject] public AreaProxyService AreaService { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;

        private IEnumerable<Area>? areas;

        protected override async Task OnInitializedAsync()
        {
            await LoadAreas();
        }

        private async Task LoadAreas()
        {
            areas = await AreaService.GetAreasAsync();
        }

        private async Task DeleteArea(int id)
        {
            var success = await AreaService.DeleteAreaAsync(id);
            if (success)
            {
                await LoadAreas();
            }
        }
    }
}