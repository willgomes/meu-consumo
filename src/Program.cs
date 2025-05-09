using Blazored.LocalStorage;
using Infrastructure;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Services;
using track_items;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IndexedDbAccessor>();
builder.Services.AddScoped<ProductService>();

builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorageAsSingleton();

var host = builder.Build();

using var scope = host.Services.CreateAsyncScope();
await using var indexedDb = scope.ServiceProvider.GetService<IndexedDbAccessor>();

if (indexedDb is not null)
    await indexedDb.InitializeAsync();

await host.RunAsync();