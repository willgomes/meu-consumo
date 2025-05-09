using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

using Models;
using MudBlazor;

using Resources;

using Services;

namespace track_items.Pages
{
    public partial class ProductForm
    {
        private ProductModel _product = new();

        private MudForm? _form;
        private bool _isLoading = false;

        [Inject]
        private ISnackbar? _snackbar { get; set; }

        [Inject]
        private ProductService? _productService { get; set; }

        [Inject] IStringLocalizer<Resource> Localizer { get; set;}

        private async Task SubmitAsync()
        {
            await _form!.Validate();
            _isLoading = true;
            StateHasChanged();

            if (_form.IsValid)
            {
                _snackbar!.Add(Localizer["ProductAddedSuccessfully"], Severity.Success, config =>
                {
                    config.ShowCloseIcon = true;
                    config.CloseAfterNavigation = true;
                });

                await _productService!.AddProductAsync(_product);

                Cancel();
            }

            _isLoading = false;
            StateHasChanged();
        }

        private void Cancel() => _product = new ProductModel();
    }
}