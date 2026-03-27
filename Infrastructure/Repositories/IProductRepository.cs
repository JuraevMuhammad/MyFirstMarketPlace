using Application.Filter;
using Domain.Entities;

namespace Infrastructure.Repositories;

public interface IProductRepository
{
    Task<int> CreateProduct(Product product);
    Task<int> UpdateProduct(Product product);
    Task<List<Product>?> GetFilterProducts(ProductFilter filter);
    Task<Product?> GetProductById(int id);
    Task<List<Product>?> GetMyProducts(int id);
}