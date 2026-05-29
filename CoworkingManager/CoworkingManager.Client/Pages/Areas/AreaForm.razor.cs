using System.Threading.Tasks;
using CoworkingManager.Models;
using CoworkingManager.Client.Services;
using Microsoft.AspNetCore.Components;

namespace CoworkingManager.Client.Pages.Areas
{
    public partial class AreaForm : ComponentBase
    {
        [Inject] public AreaProxyService AreaService { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;

        [Parameter] public int? Id { get; set; }

        private Area? area;
        private string? errorMessage;
        private bool IsEditMode => Id.HasValue;

        protected override async Task OnInitializedAsync()
        {
            if (IsEditMode)
            {
                area = await AreaService.GetAreaByIdAsync(Id!.Value);
                if (area == null)
                {
                    NavigationManager.NavigateTo("/not-found");
                }
            }
            else
            {
                area = new Area();
            }
        }

        private async Task HandleSubmit()
        {
            errorMessage = null;

            if (IsEditMode)
            {
                var success = await AreaService.UpdateAreaAsync(area!);
                if (success) NavigationManager.NavigateTo("/areas");
                else errorMessage = "An error occurred while updating the area.";
            }
            else
            {
                var result = await AreaService.CreateAreaAsync(area!);
                if (result.IsSuccess) NavigationManager.NavigateTo("/areas");
                else errorMessage = result.ErrorMessage;
            }
        }
    }
}