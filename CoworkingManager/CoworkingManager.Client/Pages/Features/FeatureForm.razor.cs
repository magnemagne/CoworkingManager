using System.Threading.Tasks;
using CoworkingManager.Models;
using CoworkingManager.Client.Services;
using Microsoft.AspNetCore.Components;

namespace CoworkingManager.Client.Pages.Features
{
    public partial class FeatureForm : ComponentBase
    {
        [Inject] public FeatureProxyService FeatureService { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;

        [Parameter] public int? Id { get; set; }

        private Feature? feature;
        private string? errorMessage;
        private bool IsEditMode => Id.HasValue;

        protected override async Task OnInitializedAsync()
        {
            if (IsEditMode)
            {
                feature = await FeatureService.GetFeatureByIdAsync(Id!.Value);
                if (feature == null)
                {
                    NavigationManager.NavigateTo("/not-found");
                }
            }
            else
            {
                feature = new Feature();
            }
        }

        private async Task HandleSubmit()
        {
            errorMessage = null;

            if (IsEditMode)
            {
                var success = await FeatureService.UpdateFeatureAsync(feature!);
                if (success) NavigationManager.NavigateTo("/features");
                else errorMessage = "An error occurred while updating the feature.";
            }
            else
            {
                var result = await FeatureService.CreateFeatureAsync(feature!);
                if (result.IsSuccess) NavigationManager.NavigateTo("/features");
                else errorMessage = result.ErrorMessage;
            }
        }
    }
}