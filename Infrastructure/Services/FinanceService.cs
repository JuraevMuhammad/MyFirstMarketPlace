using System.Net;
using System.Security.Claims;
using Application.DTOs.Finance;
using Application.DTOs.ItemFinance;
using Application.Filter;
using Application.Interfaces;
using Application.Responses;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public record DailyFinancePoint(string Date, decimal Income, decimal Expenses);
public class FinanceService : IFinanceService
{
    private readonly IFinanceRepository _repository;
    private readonly IOrderRepository _orderRepository;
    private readonly IHttpContextAccessor _accessor;

    public FinanceService(IFinanceRepository repository,
        IHttpContextAccessor accessor,
        IOrderRepository orderRepository)
    {
        _repository = repository;
        _orderRepository = orderRepository;
        _accessor = accessor;
    }

    #region GetMyFinance

    public async Task<Response<GetFinance>> GetFinance()
    {
        var userId = _accessor.HttpContext?.User.FindFirstValue("userId");
        if(!int.TryParse(userId, out var id))
            return new Response<GetFinance>(HttpStatusCode.Unauthorized, "invalid token");
        
        var finance = await _repository.GetById(id);
        var itemFinance = await _repository.GetItemFinances(id);
        
        if(finance == null)
            return new Response<GetFinance>(HttpStatusCode.NotFound, "not found");

        var getFinance = new GetFinance()
        {
            Id = finance.Id,
            CompletedOrders = finance.CompletedOrders,
            TotalOrders = finance.TotalOrders,
            NewOrders = finance.NewOrders,
            TotalRevenue = finance.TotalRevenue,
            CancelledOrders = finance.CancelledOrders,
            Expenses = finance.Expenses,
            Income = finance.Income,
            Items = itemFinance?.Select(x => new GetItemFinance()
            {
                Amount = x.Amount,
                Status = x.Status,
                CreatedAt = x.CreatedAt
            }).ToList()
        };
        return new Response<GetFinance>(getFinance);
    }

    #endregion

    #region GetAllFinance

    public async Task<Response<List<GetFinance>>> GetAllFinances()
    {
        var finances = await _repository.GetAll();
        
        if(finances == null || finances.Count == 0)
            return new Response<List<GetFinance>>(HttpStatusCode.NotFound, "not found");
        
        var getFinances = finances.Select(x => new GetFinance()
        {
            Id = x.Id,
            CompletedOrders = x.CompletedOrders,
            TotalOrders = x.TotalOrders,
            NewOrders = x.NewOrders,
            TotalRevenue = x.TotalRevenue,
            CancelledOrders = x.CancelledOrders,
            Expenses = x.Expenses,
            Income = x.Income
        }).ToList();
        
        return new Response<List<GetFinance>>(getFinances);
    }

    #endregion

    #region CreateItemFinance

    public async Task<Response<string>> CreateItemFinance(CreateItemFinance dto)
    {
        var id = _accessor.HttpContext?.User.FindFirstValue("userId");
        
        if(!int.TryParse(id, out var userId))
            return new Response<string>(HttpStatusCode.Unauthorized, "invalid token");
        
        if (dto.Amount <= 0)
            return new Response<string>(HttpStatusCode.BadRequest, "amount must be greater than 0");
        
        var finance = await _repository.GetById(userId);
        
        if (finance == null)
            return new Response<string>(HttpStatusCode.NotFound, "not found");
        
        var create = new ItemFinance()
        {
            FinanceId = finance.Id,
            Amount = dto.Amount,
            Status = dto.Status,
        };

        switch (dto.Status)
        {
            case FinanceStatus.Expense:
                finance.Expenses += create.Amount;
                finance.TotalRevenue -= create.Amount;
                break;
            case FinanceStatus.Income:
                finance.Income += create.Amount;
                finance.TotalRevenue += create.Amount;
                break;
        }

        var result = await _repository.CreateItemFinance(create, finance);
        return result > 0
            ? new Response<string>(HttpStatusCode.Created, "created item finance")
            : new Response<string>(HttpStatusCode.BadRequest, "error");
    }

    #endregion

    public async Task<PaginationResponse<List<GetItemFinance>>> GetItemFinanceFilter(FinanceFilter filter)
    {
        var itemFinances = await _repository.GetFinanceFilter(filter);
        
        if(itemFinances == null || itemFinances.Count == 0)
            return new PaginationResponse<List<GetItemFinance>>(HttpStatusCode.NotFound, "not found");

        var getItemFinance = itemFinances.Select(x => new GetItemFinance()
        {
            Id = x.Id,
            Amount = x.Amount,
            Status = x.Status,
            CreatedAt = x.CreatedAt
        }).ToList();

        var totalRecord = getItemFinance.Count;

        return new PaginationResponse<List<GetItemFinance>>(filter.PageNumber, filter.PageSize, totalRecord, getItemFinance);
    }

    public async Task<Response<string>> UpdateItemFinance(int id, UpdateItemFinance dto)
    {
        throw new  NotImplementedException();
    }

    public async Task<Response<List<ExpenseAndIncome>>> GetExpenseAndIncome(FinanceFilter filter)
    {
        var id = _accessor.HttpContext?.User.FindFirstValue("userId");
        if (!int.TryParse(id, out var userId))
            return new Response<List<ExpenseAndIncome>>(HttpStatusCode.Unauthorized, "invalid token");
    
        // логика дат
        var from = (filter.Start ?? DateTime.UtcNow.AddDays(-7)).Date;
        var to = (filter.Finish ?? DateTime.UtcNow).Date;
    
        if (from > to)
            return new Response<List<ExpenseAndIncome>>(HttpStatusCode.BadRequest, "invalid date range");
    
        var finance = await _repository.GetById(userId);
        if (finance == null)
            return new Response<List<ExpenseAndIncome>>(HttpStatusCode.NotFound, "finance not found");
    
        var orders = await _orderRepository.GetOrders();
        var expenses = await _repository.GetItemFinances(finance.Id);
    
        // INCOME FROM ORDERS 
        var incomeFromOrders = orders
            .Where(x => x.CreatedAt.Date >= from && x.CreatedAt.Date <= to && x.Status == OrderStatus.Completed)
            .GroupBy(x => x.CreatedAt.Date)
            .Select(x => new
            {
                Date = x.Key,
                Total = x.Sum(s => s.Sum)
            });
    
        // INCOME FROM FINANCE 
        var incomeFromFinance = expenses
            .Where(x => x.CreatedAt.Date >= from && x.CreatedAt.Date <= to && x.Status == FinanceStatus.Income)
            .GroupBy(x => x.CreatedAt.Date)
            .Select(x => new
            {
                Date = x.Key,
                Total = x.Sum(s => s.Amount)
            });
    
        // COMBINE INCOME
        var income = incomeFromOrders
            .Concat(incomeFromFinance)
            .GroupBy(x => x.Date)
            .Select(x => new
            {
                Date = x.Key,
                Total = x.Sum(v => v.Total)
            })
            .ToDictionary(x => x.Date, x => x.Total);
    
        //  EXPENSE 
        var expense = expenses
            .Where(x => x.CreatedAt.Date >= from && x.CreatedAt.Date <= to && x.Status == FinanceStatus.Expense)
            .GroupBy(x => x.CreatedAt.Date)
            .Select(x => new
            {
                Date = x.Key,
                Total = x.Sum(s => s.Amount)
            })
            .ToDictionary(x => x.Date, x => x.Total);
    
        //  BUILD RESULT
        var result = new List<ExpenseAndIncome>();
    
        for (var day = from; day <= to; day = day.AddDays(1))
        {
            income.TryGetValue(day, out var incomeValue);
            expense.TryGetValue(day, out var expenseValue);
    
            result.Add(new ExpenseAndIncome
            {
                Date = day.ToString("yyyy-MM-dd"),
                Income = incomeValue,
                Expense = expenseValue
            });
        }
    
        return new Response<List<ExpenseAndIncome>>(result);
    }
}