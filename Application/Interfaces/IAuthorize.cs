using Application.DTOs.User;
using Application.Responses;
using Domain.Entities;

namespace Application.Interfaces;

public interface IAuthorize
{
    Task<Response<string>> CreatedUser(CreatedUser dto);
    Task<Response<string>> LoginUser(LoginUser dto);
}