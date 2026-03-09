using System.Drawing;
using System.Net;
using Application.DTOs.ItemProduct;
using Application.DTOs.Product;
using Application.Filter;
using Application.Interfaces;
using Application.Responses;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Repositories;
using Infrastructure.SaveFile;

namespace Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IFileStorage _file;

    public ProductService(IProductRepository repository, IFileStorage file)
    {
        _repository = repository;
        _file = file;
    }

    public Task<PaginationResponse<List<GetProduct>>> GetFilterProduct(ProductFilter filter)
    {
        throw new NotImplementedException();
    }

    #region CreateProduct

    public async Task<Response<string>> CreateProduct(CreatedProduct dto)
    {
        var product = new Product()
        {
            CategoryId = dto.CategoryId,
            UserId = dto.UserId,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            FileImage = await _file.SaveFilesAsync(dto.Images, "product")
        };

        var result =await _repository.CreateProduct(product);
        return new Response<string>(HttpStatusCode.Created, "created product");
    }

    #endregion

    public Task<Response<string>> UpdateProduct(int id, UpdatedProduct dto)
    {
        throw new NotImplementedException();
    }

    public Task<Response<string>> DeleteProduct(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Response<GetProduct>> GetProductById(int id)
    {
        throw new NotImplementedException();
    }
}