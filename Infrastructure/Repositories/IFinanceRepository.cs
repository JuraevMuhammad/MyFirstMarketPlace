using Application.Filter;
using Domain.Entities;

namespace Infrastructure.Repositories;

public interface IFinanceRepository
{
    Task<List<Finance>?> GetAll();
    Task<Finance?> GetById(int id);
    Task<List<ItemFinance>?> GetItemFinances(int id);
    Task<int> CreateItemFinance(ItemFinance dto, Finance finance);
    Task<List<ItemFinance>?> GetFinanceFilter(FinanceFilter filter);
}