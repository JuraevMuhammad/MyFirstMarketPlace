using System.Net;
using Application.DTOs.ItemProduct;
using Application.Interfaces;
using Application.Responses;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Repositories;
using Infrastructure.SaveFile;

namespace Infrastructure.Services;

public class ItemProductService : IItemProductService
{
    private readonly IItemProductRepository _repository;
    private readonly IFileStorage _file;

    public ItemProductService(IItemProductRepository repository, IFileStorage file)
    {
        _repository = repository;
        _file = file;
    }
    
    public async Task<Response<string>> CreateItemProduct(CreatedItemProduct dto)
    {
        var itemProduct = new ItemProduct()
        {
            ProductId = dto.ProductId,
            Size = dto.Size ?? SizeProduct.NoSize,
            ColorProduct = dto.Color ?? ColorProduct.NoColor,
            Quantity = dto.Quantity,
            Images = await _file.SaveFilesAsync(dto.Images, "product")
        };

        var result = await _repository.CreateItemProduct(itemProduct);
        return result > 0
            ? new Response<string>(HttpStatusCode.Created, "ItemProduct created successfully")
            : new Response<string>(HttpStatusCode. BadRequest, "ItemProduct not created successfully");
    }

    #region UpdateItemProduct

    public async Task<Response<string>> UpdateItemProduct(int id, UpdateItemProduct dto)
    {
        var product = await _repository.GetItemProduct(id);
        if (product == null)
            return new Response<string>(HttpStatusCode.NotFound, "ItemProduct not found");
        product.Size = dto.Size ?? product.Size;
        product.ColorProduct = dto.Color ?? product.ColorProduct;
        product.Quantity = dto.Quantity ?? product.Quantity;
        var result = await _repository.UpdateItemProduct(product);
        return result > 0
            ? new Response<string>(HttpStatusCode.Accepted, "ItemProduct updated successfully")
            : new Response<string>(HttpStatusCode.BadRequest, "ItemProduct not updated successfully");
    }

    #endregion

    #region DeleteItemProduct

    public async Task<Response<string>> DeleteItemProduct(int id)
    {
        var product = await _repository.GetItemProduct(id);
        if (product == null)
            return new Response<string>(HttpStatusCode.NotFound, "not found");
        product.IsDeleted = true;
        var result = await _repository.UpdateItemProduct(product);
        return result > 0
            ? new Response<string>(HttpStatusCode.NoContent, "ItemProduct deleted successfully")
            : new Response<string>(HttpStatusCode.BadRequest, "ItemProduct not deleted successfully");
    }

    #endregion
}