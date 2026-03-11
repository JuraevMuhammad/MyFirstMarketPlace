using Application.Interfaces;
using Application.Responses;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class FinanceService : IFinanceService
{
    private readonly IFinanceRepository _repository;

    public FinanceService(IFinanceRepository repository)
    {
        _repository = repository;
    }
    
}