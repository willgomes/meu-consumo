using Infrastructure;
using Models;

namespace Services;

public class ProductService(
    IndexedDbAccessor indexedDbAccessor
)
{
    const string PRODUCT_COLLECTION_NAME = "products";
    const string TYPE_COLLECTION_NAME = "types";

    public Task AddProductAsync(ProductModel product) => indexedDbAccessor.SetValueAsync(PRODUCT_COLLECTION_NAME, product);

    public Task<IEnumerable<ProductModel>> GetProductsAsync() => indexedDbAccessor.GetAllAsync<ProductModel>(PRODUCT_COLLECTION_NAME);

    public Task DeleteProductAsync(Guid id) => indexedDbAccessor.DeleteAsync(PRODUCT_COLLECTION_NAME, id);

    public Task<ProductModel> GetProductAsync(Guid id) => indexedDbAccessor.GetValueAsync<ProductModel>(PRODUCT_COLLECTION_NAME, id);

    public Task AddTypeAsync(TypeModel type) => indexedDbAccessor.SetValueAsync(TYPE_COLLECTION_NAME, type);

    public Task<IEnumerable<TypeModel>> GetTypesAsync() => indexedDbAccessor.GetAllAsync<TypeModel>(TYPE_COLLECTION_NAME);

    public Task DeleteTypeAsync(Guid id) => indexedDbAccessor.DeleteAsync(TYPE_COLLECTION_NAME, id);

    public Task<TypeModel> GetTypeAsync(Guid id) => indexedDbAccessor.GetValueAsync<TypeModel>(TYPE_COLLECTION_NAME, id);
}