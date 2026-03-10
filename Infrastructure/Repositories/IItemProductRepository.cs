using Domain.Entities;

namespace Infrastructure.Repositories;

public interface IItemProductRepository
{
    Task<int> CreateItemProduct(ItemProduct product);
    Task<int> UpdateItemProduct(ItemProduct product);
    Task<ItemProduct?> GetItemProduct(int id);
    
}