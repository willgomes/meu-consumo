using Infrastructure;
using Models;

namespace Services;

public class ProductService(
    IndexedDbAccessor indexedDbAccessor
)
{
    const string COLLECTION_NAME = "products";

    public Task AddProductAsync(ProductModel product) => indexedDbAccessor.SetValueAsync(COLLECTION_NAME, product);
    
    public Task<IEnumerable<ProductModel>> GetProductsAsync() => indexedDbAccessor.GetAllAsync<ProductModel>(COLLECTION_NAME);

    public Task DeleteProductAsync(Guid id) => indexedDbAccessor.DeleteAsync(COLLECTION_NAME, id);

    public Task<ProductModel> GetProductAsync(Guid id) => indexedDbAccessor.GetValueAsync<ProductModel>(COLLECTION_NAME, id);
}