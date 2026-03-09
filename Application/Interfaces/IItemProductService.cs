using Application.DTOs.ItemProduct;
using Application.Responses;

namespace Application.Interfaces;

public interface IItemProductService
{
    Task<Response<string>> CreateItemProduct(CreatedItemProduct dto);
    
}