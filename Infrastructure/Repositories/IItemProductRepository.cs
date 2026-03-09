using Domain.Entities;

namespace Infrastructure.Repositories;

public interface IItemProductRepository
{
    Task<int> CreateItemProduct(ItemProduct product);
}