using System.Net;
using Application.DTOs.ItemProduct;
using Application.Interfaces;
using Application.Responses;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class ItemProductService : IItemProductService
{
    private readonly IItemProductRepository _repository;

    public ItemProductService(IItemProductRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Response<string>> CreateItemProduct(CreatedItemProduct dto)
    {
        var itemProduct = new ItemProduct()
        {
            ProductId = dto.ProductId,
            Size = dto.Size ?? SizeProduct.NoSize,
            ColorProduct = dto.Color ?? ColorProduct.NoColor,
            Quantity = dto.Quantity,
        };

        var result = await _repository.CreateItemProduct(itemProduct);
        return result > 0
            ? new Response<string>(HttpStatusCode.Created, "ItemProduct created successfully")
            : new Response<string>(HttpStatusCode. BadRequest, "ItemProduct not created successfully");
    }
}