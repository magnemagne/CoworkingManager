using System.Collections.Generic;
using System.Threading.Tasks;
using CoworkingManager.Models;
using CoworkingManager.Client.Services;
using Microsoft.AspNetCore.Components;

namespace CoworkingManager.Client.Pages.Workstations
{
    public partial class WorkstationForm : ComponentBase
    {
        [Inject] public WorkstationProxyService WorkstationService { get; set; } = default!;
        [Inject] public AreaProxyService AreaService { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;

        [Parameter] public int? Id { get; set; }

        private Workstation? workstation;
        private IEnumerable<Area>? availableAreas;
        private string? errorMessage;
        private int selectedAreaId { get; set; }
        private bool IsEditMode => Id.HasValue;

        protected override async Task OnInitializedAsync()
        {
            availableAreas = await AreaService.GetAreasAsync() ?? new List<Area>();

            if (IsEditMode)
            {
                workstation = await WorkstationService.GetWorkstationByIdAsync(Id!.Value);
                if (workstation == null)
                {
                    NavigationManager.NavigateTo("/not-found");
                }
                else
                {
                    selectedAreaId = workstation.Area?.IdArea ?? 0;
                }
            }
            else
            {
                workstation = new Workstation();
            }
        }

        private async Task HandleSubmit()
        {
            errorMessage = null;

            workstation!.Area ??= new Area();
            workstation.Area.IdArea = selectedAreaId;

            if (selectedAreaId == 0)
            {
                errorMessage = "Please select an Area.";
                return;
            }

            if (IsEditMode)
            {
                var success = await WorkstationService.UpdateWorkstationAsync(workstation);
                if (success) NavigationManager.NavigateTo("/workstations");
                else errorMessage = "An error occurred while updating the workstation.";
            }
            else
            {
                var result = await WorkstationService.CreateWorkstationAsync(workstation);
                if (result.IsSuccess) NavigationManager.NavigateTo("/workstations");
                else errorMessage = result.ErrorMessage;
            }
        }
    }
}