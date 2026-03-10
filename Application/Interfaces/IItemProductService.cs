using Application.DTOs.ItemProduct;
using Application.Responses;

namespace Application.Interfaces;

public interface IItemProductService
{
    Task<Response<string>> CreateItemProduct(CreatedItemProduct dto);
    Task<Response<string>> UpdateItemProduct(int id, UpdateItemProduct dto);
    Task<Response<string>> DeleteItemProduct(int id);
}