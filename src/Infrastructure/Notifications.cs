using Microsoft.JSInterop;
using System.Text.Json;

namespace Infrastructure;

public class Notifications(IJSRuntime jsRuntime) : IAsyncDisposable
{
    private readonly IJSRuntime _jsRuntime = jsRuntime;
    private IJSObjectReference? _module;

    private async Task EnsureModuleImportedAsync() => _module ??= await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/Notification.js");

    const string NotificationPermissionDenied = "denied";

    public async Task<bool> RequestPermissionAsync()
    {
        await EnsureModuleImportedAsync();

        try
        {
            var result = await _module!.InvokeAsync<JsonElement>("requestPermission");

            return result.ValueKind == JsonValueKind.True || (result.ValueKind == JsonValueKind.String && result.GetString() != NotificationPermissionDenied);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error requesting notification permission: {ex.Message}");
            return false;
        }
    }

    public async Task ShowNotificationAsync(string title, NotificationOptions options)
    {
        await EnsureModuleImportedAsync();
        try {
            Console.WriteLine($"Showing notification: {title}");
            await _module!.InvokeVoidAsync("show", title, options);
            Console.WriteLine("Notification show method invoked");
        }
        catch (Exception ex) {
            Console.WriteLine($"Error showing notification: {ex.Message}");
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
            await _module.DisposeAsync();

    }
}

public class NotificationOptions
{
    public string? Body { get; set; }
    public string? Icon { get; set; }
    public string? Badge { get; set; }
    public int[]? Vibrate { get; set; }
    public string? Tag { get; set; }
    public object? Data { get; set; }
}