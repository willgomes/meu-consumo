using Blazored.LocalStorage;

using Infrastructure;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

using Models;

using MudBlazor;

using Resources;

using Services;

namespace track_items.Pages;

public partial class Home
{
    [Inject] ILocalStorageService? LocalStorage { get; set; }
    [Inject] Notifications? Notifications { get; set; }
    [Inject] IStringLocalizer<Resource>? Localizer { get; set; }
    [Inject] ProductService? ProductService { get; set; }

    private IEnumerable<GroupAverageModel> _groupAverage = [];

    private string NotificationIcon => IsNotificationEnabled ? Icons.Material.Filled.NotificationsActive :
    Icons.Material.Filled.NotificationsOff;

    private bool _isNotificationEnabled;
    public bool IsNotificationEnabled
    {
        get => _isNotificationEnabled;
        set
        {
            if (_isNotificationEnabled != value)
            {
                _isNotificationEnabled = value;

                _ = Task.Run(EnableNotificationsAsync);
            }
        }
    }

    protected override async Task OnInitializedAsync()
    {
        string? notificationRequest = await LocalStorage!.GetItemAsStringAsync("notificationRequest");

        _ = bool.TryParse(notificationRequest, out bool hasAuthorizedNotification);

        _isNotificationEnabled = hasAuthorizedNotification;

        _groupAverage = await GetGroupAverageUsageAsync();

    }

    private async Task EnableNotificationsAsync()
    {
        bool result = await Notifications!.RequestPermissionAsync();

        await LocalStorage!.SetItemAsync("notificationRequest", result);
    }

    private async Task<IEnumerable<GroupAverageModel>> GetGroupAverageUsageAsync()
    {
        IEnumerable<ProductModel> products = await ProductService!.GetProductsAsync();

        return products.Where(_ => _.IsChecked == true)
            .GroupBy(p => p.Type)
            .Select(g => new GroupAverageModel
            {
                Name = g.Key,
                AverageTime = g.Average(_ => _.GetTotalDiffTime()),
                AveragePrice = g.Average(_ => _.Price ?? 0m),
                PriceVariation =
                        (g.Min(p => p.Price ?? 0m) / g.Max(p => p.Price ?? 0m)) * 100
            });
    }

}
