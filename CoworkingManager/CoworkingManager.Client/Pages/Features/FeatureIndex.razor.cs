using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoworkingManager.Models;
using CoworkingManager.Client.Services;
using Microsoft.AspNetCore.Components;

namespace CoworkingManager.Client.Pages.Features
{
    public partial class FeatureIndex : ComponentBase
    {
        [Inject] public FeatureProxyService FeatureService { get; set; } = default!;

        private IEnumerable<Feature>? features;

        protected override async Task OnInitializedAsync()
        {
            await LoadFeatures();
        }

        private async Task LoadFeatures()
        {
            features = await FeatureService.GetFeaturesAsync();
        }

        private async Task DeleteFeature(int id)
        {
            var success = await FeatureService.DeleteFeatureAsync(id);
            if (success)
            {
                await LoadFeatures();
            }
        }
    }
}