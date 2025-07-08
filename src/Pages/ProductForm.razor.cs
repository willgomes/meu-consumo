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

        [Parameter]
        public Guid ProductId { get; set; }

        private MudForm? _form;
        private bool _isLoading = false;
        private bool _autoCompleteHidden = false;
        private string _typeName = string.Empty;

        [Inject]
        private ISnackbar? Snackbar { get; set; }

        [Inject]
        private ProductService? ProductService { get; set; }

        [Inject] IStringLocalizer<Resource>? Localizer { get; set; }

        [Inject] NavigationManager? NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (IsEditMode)
                _product = await ProductService!.GetProductAsync(ProductId);
        }

        private async Task SubmitAsync()
        {
            await _form!.Validate();
            _isLoading = true;
            StateHasChanged();

            if (_form.IsValid)
            {
                Snackbar!.Add(Localizer!["ProductAddedSuccessfully"], Severity.Success, config =>
                {
                    config.ShowCloseIcon = true;
                    config.CloseAfterNavigation = true;
                });

                await ProductService!.AddProductAsync(_product);

                if (!IsEditMode)
                    Cancel();
            }

            _isLoading = false;
            StateHasChanged();
        }

        private bool IsEditMode => ProductId != Guid.Empty;

        private void Cancel()
        {
            if (IsEditMode)
                NavigationManager!.NavigateTo("/product");
            else
                _product = new ProductModel();
        }

        private async Task AddNewTypeAsync()
        {
            TypeModel type = new() { Name = _typeName };

            IEnumerable<string> result = await SearchAsync(type.Name, CancellationToken.None);

            if (result.Any())
                Snackbar!.Add(Localizer!["TypeAlreadyExists"], Severity.Warning);
            else
            {
                await ProductService!.AddTypeAsync(type);
                Snackbar!.Add(Localizer!["TypeAddedSuccessfully"], Severity.Success);
            }

            AddTypeToggleButton(false);
        }

        private void AddTypeToggleButton(bool isHidden)
        {
            _autoCompleteHidden = isHidden;
            StateHasChanged();
        }

        private async Task<IEnumerable<string>> SearchAsync(string value, CancellationToken cancellationToken)
        {
            IEnumerable<TypeModel> types = await ProductService!.GetTypesAsync();

            if (string.IsNullOrEmpty(value))
                return types.Select(_ => _.Name);

            return types
                .Where(_ => _.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(_ => _.Name) ?? [];
        }
    }
}