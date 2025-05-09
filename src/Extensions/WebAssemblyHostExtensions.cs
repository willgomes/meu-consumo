using System.Globalization;

using Blazored.LocalStorage;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using Shared;

namespace Extensions;

public static class WebAssemblyHostExtensions
{
    public async static Task SetDefaultCultureAsync(this WebAssemblyHost host)
    {
        var localStorage = host.Services.GetRequiredService<ILocalStorageService>();
        var cultureString = await localStorage.GetItemAsync<string>(LocalizerSettings.CULTURE_KEY);
 
        CultureInfo cultureInfo = !string.IsNullOrWhiteSpace(cultureString) ? new CultureInfo(cultureString) : new CultureInfo("pt-BR");
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
    }
}