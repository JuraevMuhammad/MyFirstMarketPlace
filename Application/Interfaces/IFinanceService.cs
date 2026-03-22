using Application.DTOs.Finance;
using Application.DTOs.ItemFinance;
using Application.Filter;
using Application.Responses;

namespace Application.Interfaces;

public interface IFinanceService
{
    Task<Response<GetFinance>> GetFinance();
    Task<Response<List<GetFinance>>> GetAllFinances();
    Task<Response<string>> CreateItemFinance(CreateItemFinance dto);
    Task<PaginationResponse<List<GetItemFinance>>> GetItemFinanceFilter(FinanceFilter filter);
    Task<Response<string>> UpdateItemFinance(int id, UpdateItemFinance dto);
    Task<Response<List<ExpenseAndIncome>>> GetExpenseAndIncome(FinanceFilter filter);
}