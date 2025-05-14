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
        [Inject] IStringLocalizer<Resource> Localizer { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }

        private IEnumerable<ProductModel>? _products = [];

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadProductsAsync();
        }

        private async Task DeleteProductAsync(Guid id)
        {
            await ProductService!.DeleteProductAsync(id);

            Snackbar!.Add("Product deleted", Severity.Success);

            await LoadProductsAsync();
        }

        private void EditProduct(Guid id) => NavigationManager.NavigateTo($"/product-form/{id}");

        private async Task LoadProductsAsync() => _products = await ProductService!.GetProductsAsync();

        private async Task CheckProductAsync(ProductModel product)
        {
            product.UpdatedAt = DateTime.UtcNow;
            await ProductService!.AddProductAsync(product);
        }

    }
}