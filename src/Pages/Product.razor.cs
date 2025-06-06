using Blazored.LocalStorage;

using Infrastructure;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

using Models;

using MudBlazor;

using Resources;

using Services;

namespace track_items.Pages
{
    public partial class Product
    {
        [Inject] ProductService? ProductService { get; set; }
        [Inject] ISnackbar? Snackbar { get; set; }
        [Inject] IStringLocalizer<Resource>? Localizer { get; set; }
        [Inject] NavigationManager? NavigationManager { get; set; }
        [Inject] Notifications? Notifications { get; set; }
        [Inject] ILocalStorageService? LocalStorage { get; set; }

        private IEnumerable<ProductModel> _products = [];

        private List<ProductModel> _filteredProducts = [];

        private bool _showCheckedProductsOnly = false;
        public bool ShowCheckedProductsOnly
        {
            get => _showCheckedProductsOnly;
            set
            {
                _showCheckedProductsOnly = value;
                ApplyProductFilters();
            }
        }
        private string _searchText = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadProductsAsync();
        }

        private async Task DeleteProductAsync(Guid id)
        {
            await ProductService!.DeleteProductAsync(id);

            Snackbar!.Add(Localizer!["ProductDeleted"], Severity.Success);

            await LoadProductsAsync();
        }

        private void EditProduct(Guid id) => NavigationManager!.NavigateTo($"/product-form/{id}");

        private async Task LoadProductsAsync()
        {
            _products = await ProductService!.GetProductsAsync();
            _products = _products.Where(p => p.IsChecked == _showCheckedProductsOnly);
            _filteredProducts = [.. _products];

            if (_products.Any())
            {
                int expired = _products
                    .AsParallel()
                    .Count(p => p.ExpirationDate <= DateTime.UtcNow.AddDays(5) && (p.IsChecked == false || p.UpdatedAt == null) && p.ExpirationDate >= DateTime.UtcNow);

                if (expired > 0)
                {
                    await NotifyExpiredProducts(expired);
                }
            }

            async Task NotifyExpiredProducts(int expired)
            {
                var lastNotification = await LocalStorage!.GetItemAsync<DateTime?>("last-notification");

                if (lastNotification == null || lastNotification < DateTime.UtcNow.AddDays(-1))
                {
                    await LocalStorage.SetItemAsync("last-notification", DateTime.UtcNow);
                    await CheckProductAsync(_products.First(p => p.ExpirationDate <= DateTime.UtcNow.AddDays(5) && p.ExpirationDate >= DateTime.UtcNow));

                    Snackbar!.Add(Localizer!["TotalExpirationItens", expired], Severity.Warning);

                    await Notifications!.ShowNotificationAsync(
                        Localizer!["ProductExpirationWarning"],
                        new NotificationOptions
                        {
                            Body = Localizer["TotalExpirationItens", expired],
                            Icon = "images/warning-icon.png",
                            Badge = "images/expired-product-badge.png",
                            Vibrate = [200, 100, 200],
                            Tag = "expiration-warning",
                            Data = new { url = NavigationManager!.Uri }
                        });
                }
            }
        }

        private async Task CheckProductAsync(ProductModel product)
        {
            product.UpdatedAt = DateTime.UtcNow;
            product.IsChecked = true;
            await ProductService!.AddProductAsync(product);
        }

        private async Task<IEnumerable<ProductModel>> SearchProducts(string value, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(value))
                return _products ?? [];

            IEnumerable<ProductModel> result = _products?.Where(p =>
                   p.GetHumanizedName()!.Contains(value, StringComparison.OrdinalIgnoreCase))
                   ?? [];

            return await Task.FromResult(result);
        }

        private void HandleProductSelected(ProductModel? product) => _filteredProducts = product is null ? [.. _products] : [product];

        private void OnSearchTextChanged(string searchText)
        {
            _searchText = searchText;
            ApplyProductFilters();
        }

        private void ApplyProductFilters()
        {
            if (_products == null) return;

            _filteredProducts = [.. _products
                .Where(p => string.IsNullOrEmpty(_searchText) ||
                    p.Name?.Contains(_searchText, StringComparison.OrdinalIgnoreCase) == true)
                .Where(p => p.IsChecked == _showCheckedProductsOnly)];

            StateHasChanged();
        }
    }
}