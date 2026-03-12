using Application.DTOs.Finance;
using Application.DTOs.ItemFinance;
using Application.Responses;

namespace Application.Interfaces;

public interface IFinanceService
{
    Task<Response<GetFinance>> GetFinance();
    Task<Response<List<GetFinance>>> GetAllFinances();
    Task<Response<string>> CreateItemFinance(CreateItemFinance dto);
}