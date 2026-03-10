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

    #region GetPaginationProduct

    public async Task<PaginationResponse<List<GetProduct>>> GetFilterProduct(ProductFilter filter)
    {
        var products = await _repository.GetFilterProducts(filter);
        
        if(products == null)
            return new PaginationResponse<List<GetProduct>>(HttpStatusCode.NotFound, "not found");
        
        var getProduct = products.Select(x => new GetProduct()
        {
            CategoryId = x.CategoryId,
            UserId = x.UserId,
            Name = x.Name,
            Description = x.Description,
            Price = x.Price,
        }).ToList();
        
        var totalRecord = getProduct.Count;
        
        return new PaginationResponse<List<GetProduct>>(filter.PageNumber,  filter.PageSize, totalRecord, getProduct);
    }

    #endregion

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

    #region UpdateProduct

    public async Task<Response<string>> UpdateProduct(int id, UpdatedProduct dto)
    {
        var res = await _repository.GetProductById(id);
        if(res == null)
            return new Response<string>(HttpStatusCode.NotFound, "not found");
        res.Name = dto.Name ?? res.Name;
        res.Description = dto.Description ?? res.Description;
        res.Price = dto.Price ?? res.Price;
        res.CategoryId = dto.CategoryId ?? res.CategoryId;
        var result = await _repository.UpdateProduct(res);
        return result > 0
            ? new Response<string>(HttpStatusCode.OK, "updated product")
            : new Response<string>(HttpStatusCode.BadRequest, "updated product failed");
    }

    #endregion

    #region DeleteProdcut

    public async Task<Response<string>> DeleteProduct(int id)
    {
        var product = await _repository.GetProductById(id);
        if(product == null)
            return new Response<string>(HttpStatusCode.NotFound, "not found");
        product.IsDeleted = true;
        var result = await _repository.UpdateProduct(product);
        return result > 0 
            ? new Response<string>(HttpStatusCode.NoContent, "deleted product")
            : new Response<string>(HttpStatusCode.BadRequest, "deleted product failed");
    }

    #endregion

    #region GetProductById

    public async Task<Response<GetProduct>> GetProductById(int id)
    {
        var product = await _repository.GetProductById(id);
        
        if(product == null)
            return new Response<GetProduct>(HttpStatusCode.NotFound, "not found");

        var getProduct = new GetProduct()
        {
            CategoryId = product.CategoryId,
            Description = product.Description,
            Name = product.Name,
            Price = product.Price,
            UserId = product.UserId,
        };
        return new Response<GetProduct>(getProduct);
    }

    #endregion
}