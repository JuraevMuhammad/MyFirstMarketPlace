using System.Net;
using Application.DTOs.Finance;
using Application.DTOs.ItemFinance;
using Application.Interfaces;
using Application.Responses;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class FinanceService : IFinanceService
{
    private readonly IFinanceRepository _repository;

    public FinanceService(IFinanceRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<GetFinance>> GetFinance(int id)
    {
        var finance = await _repository.GetById(id);
        var itemFinance = await _repository.GetItemFinances(id);
        
        if(itemFinance == null || finance == null)
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
            Items = itemFinance.Select(x => new GetItemFinance()
            {
                Amount = x.Amount,
                Status = x.Status,
                CreatedAt = x.CreatedAt
            }).ToList()
        };
        return new Response<GetFinance>(getFinance);
    }

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

    public async Task<Response<string>> CreateItemFinance(CreateItemFinance dto)
    {
        if (dto.Amount <= 0)
            return new Response<string>(HttpStatusCode.BadRequest, "amount must be greater than 0");
        
        var finance = await _repository.GetById(dto.FinanceId);
        
        if (finance == null)
            return new Response<string>(HttpStatusCode.NotFound, "not found");
        
        var create = new ItemFinance()
        {
            FinanceId = dto.FinanceId,
            Amount = dto.Amount,
            Status = dto.Status,
        };

        switch (dto.Status)
        {
            case FinanceStatus.Expense:
                finance.Expenses += create.Amount;
                break;
            case FinanceStatus.Income:
                finance.Income += create.Amount;
                break;
        }

        var result = await _repository.CreateItemFinance(create, finance);
        return result > 0
            ? new Response<string>(HttpStatusCode.Created, "created item finance")
            : new Response<string>(HttpStatusCode.BadRequest, "error");
    }
}