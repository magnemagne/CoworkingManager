using CoworkingManager.Client.Pages;
using CoworkingManager.Components;
using CoworkingManager.Backend.Endpoints;
using CoworkingManager.Services.Interfaces;
using CoworkingManager.Services.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddScoped<IAreaService, AreaService>();
builder.Services.AddScoped<IWorkstationService, WorkstationService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IFeatureService, FeatureService>();
builder.Services.AddScoped<IStatusService, StatusService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();

app.MapAreas();
app.MapWorkstations();
app.MapCustomers();
app.MapBookings();
app.MapFeatures();
app.MapStatuses();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(CoworkingManager.Client._Imports).Assembly);

app.Run();