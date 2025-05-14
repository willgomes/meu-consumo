using Blazored.LocalStorage;

using Extensions;

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
builder.Services.AddScoped<Notifications>();

builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorageAsSingleton();
	
builder.Services.AddLocalization();

var host = builder.Build();

using var scope = host.Services.CreateAsyncScope();
await using var indexedDb = scope.ServiceProvider.GetService<IndexedDbAccessor>();

if (indexedDb is not null)
    await indexedDb.InitializeAsync();

await host.SetDefaultCultureAsync();

await host.RunAsync();