using Microsoft.JSInterop;

namespace Infrastructure;

public class IndexedDbAccessor(IJSRuntime jsRuntime) : IAsyncDisposable, IDisposable
{
    private Lazy<IJSObjectReference> _accessorJsRef = new();
    private readonly IJSRuntime _jsRuntime = jsRuntime;
    private bool _disposed;

    public async Task InitializeAsync()
    {
        await WaitForReference();
        await _accessorJsRef.Value.InvokeVoidAsync("initialize");
    }

    private async Task WaitForReference()
    {
        if (_accessorJsRef.IsValueCreated is false)
        {
            _accessorJsRef = new(await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/js/IndexedDbStorageAccessor.js"));
        }
    }

    public async Task<T> GetValueAsync<T>(string collectionName, Guid id)
    {
        await WaitForReference();
        return await _accessorJsRef.Value.InvokeAsync<T>("get", collectionName, id);
    }

    public async Task SetValueAsync<T>(string collectionName, T value)
    {
        await WaitForReference();
        await _accessorJsRef.Value.InvokeVoidAsync("set", collectionName, value);
    }

    public async Task<IEnumerable<T>> GetAllAsync<T>(string collectionName)
    {
        await WaitForReference();
        return await _accessorJsRef.Value.InvokeAsync<IEnumerable<T>>("getAll", collectionName);
    }

    public async Task DeleteAsync(string collectionName, Guid id)
    {
        await WaitForReference();
        await _accessorJsRef.Value.InvokeVoidAsync("del", collectionName, id);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);

        Dispose(disposing: false);
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (_disposed)
            return;

        if (_accessorJsRef.IsValueCreated)
        {
            await _accessorJsRef.Value.DisposeAsync().ConfigureAwait(false);
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            if (_accessorJsRef.IsValueCreated)
            {
                // Since we can't dispose synchronously of an async resource,
                // we block on the async disposal. This is not recommended but
                // necessary to properly implement IDisposable
                _accessorJsRef.Value.DisposeAsync().AsTask()
                    .GetAwaiter().GetResult();
            }
        }

        _disposed = true;
    }
}