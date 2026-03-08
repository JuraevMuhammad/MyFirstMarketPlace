using Application.DTOs.Product;
using Application.Responses;

namespace Application.Interfaces;

public interface IProductService
{
    Task<PaginationResponse<List<GetProduct>>> GetFilterProduct();
}