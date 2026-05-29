using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoworkingManager.Models;
using CoworkingManager.Client.Services;
using Microsoft.AspNetCore.Components;

namespace CoworkingManager.Client.Pages.Customers
{
    public partial class CustomerIndex : ComponentBase
    {
        [Inject] public CustomerProxyService CustomerService { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;

        private IEnumerable<Customer>? customers;

        protected override async Task OnInitializedAsync()
        {
            await LoadCustomers();
        }

        private async Task LoadCustomers()
        {
            customers = await CustomerService.GetCustomersAsync();
        }

        private async Task DeleteCustomer(int id)
        {
            var success = await CustomerService.DeleteCustomerAsync(id);
            if (success)
            {
                await LoadCustomers();
            }
        }
    }
}