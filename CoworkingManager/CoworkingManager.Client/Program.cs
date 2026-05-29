using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using CoworkingManager.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<AreaProxyService>();
builder.Services.AddScoped<WorkstationProxyService>();
builder.Services.AddScoped<CustomerProxyService>();
builder.Services.AddScoped<BookingProxyService>();
builder.Services.AddScoped<FeatureProxyService>();
builder.Services.AddScoped<StatusProxyService>();

await builder.Build().RunAsync();