﻿using AutoMapper;
using MyToDo.Api.Models;
using MyToDo.Api.UnitOfWork;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using System.Security.Principal;

namespace MyToDo.Api.Services
{
  public class AuthService : IAuthService
  {
    private readonly IUnitOfWork worker;
    private readonly IRepository<User> repo;

    public AuthService(IUnitOfWork unitOfWork)
    {
      this.worker = unitOfWork;
      repo = unitOfWork.GetRepository<User>();
    }
    public async Task<ApiResponse> LoginAsync(string Account, string Password)
    {
      try
      {
        var user = await repo.GetFirstOrDefaultAsync(predicate: user =>
          (user.Account.Equals(Account)) && (user.Password.Equals(Password))
        );
        if (user == null)
        {
          return new ApiResponse("登录失败");
        }
        return new ApiResponse(true, user);
      }
      catch (Exception ex)
      {
        return new ApiResponse(ex.Message);
      }
    }

    public async Task<ApiResponse> RegisterAsync(User user)
    {
      try
      {
        var oldUser = await repo.GetFirstOrDefaultAsync(predicate: u =>
          u.Account.Equals(user.Account)
        );
        if (oldUser != null)
        {
          return new ApiResponse($"账号{user.Account}已存在");
        }
        user.CreatedAt = DateTime.Now;
        await repo.InsertAsync(user);
        int count = await worker.SaveChangesAsync();
        if (count > 0)
        {
          return new ApiResponse(true, user);
        }
        return new ApiResponse("新建用户失败");
      }
      catch (Exception ex)
      {
        return new ApiResponse(ex.Message);
      }

    }
  }
}
