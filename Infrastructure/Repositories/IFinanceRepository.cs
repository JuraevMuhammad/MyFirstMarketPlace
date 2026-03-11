using Domain.Entities;

namespace Infrastructure.Repositories;

public interface IFinanceRepository
{
    Task<List<Finance>?> GetAll();
    Task<Finance?> GetById(int id);
}