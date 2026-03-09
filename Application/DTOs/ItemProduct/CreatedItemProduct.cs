using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.DTOs.ItemProduct;

public class CreatedItemProduct
{
    public int ProductId { get; set; }
    public SizeProduct? Size { get; set; }
    public ColorProduct? Color { get; set; }
    public int Quantity { get; set; }
}