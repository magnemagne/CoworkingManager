using System.Threading.Tasks;
using CoworkingManager.Models;
using CoworkingManager.Client.Services;
using Microsoft.AspNetCore.Components;

namespace CoworkingManager.Client.Pages.Customers
{
    public partial class CustomerForm : ComponentBase
    {
        [Inject] public CustomerProxyService CustomerService { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;

        [Parameter] public int? Id { get; set; }

        private Customer? customer;
        private string? errorMessage;
        private bool IsEditMode => Id.HasValue;

        protected override async Task OnInitializedAsync()
        {
            if (IsEditMode)
            {
                customer = await CustomerService.GetCustomerByIdAsync(Id!.Value);
                if (customer == null)
                {
                    NavigationManager.NavigateTo("/not-found");
                }
            }
            else
            {
                customer = new Customer();
            }
        }

        private async Task HandleSubmit()
        {
            errorMessage = null;

            if (IsEditMode)
            {
                var success = await CustomerService.UpdateCustomerAsync(customer!);
                if (success) NavigationManager.NavigateTo("/customers");
                else errorMessage = "An error occurred while updating the customer.";
            }
            else
            {
                var result = await CustomerService.CreateCustomerAsync(customer!);
                if (result.IsSuccess) NavigationManager.NavigateTo("/customers");
                else errorMessage = result.ErrorMessage;
            }
        }
    }
}