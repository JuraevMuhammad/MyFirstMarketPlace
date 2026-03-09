using Application.DTOs.Product;
using Application.Filter;
using Application.Responses;

namespace Application.Interfaces;

public interface IProductService
{
    Task<PaginationResponse<List<GetProduct>>> GetFilterProduct(ProductFilter filter);
    Task<Response<string>> CreateProduct(CreatedProduct dto);
    Task<Response<string>> UpdateProduct(int id, UpdatedProduct dto);
    Task<Response<string>> DeleteProduct(int id);
    Task<Response<GetProduct>> GetProductById(int id);
}