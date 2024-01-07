using Microsoft.AspNetCore.Mvc;
using MyToDo.Api.Models;
using MyToDo.Api.Services;
using MyToDo.Api.UnitOfWork;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

namespace MyToDo.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController: ControllerBase
{
  private readonly IAuthService service;

  public AuthController(IAuthService service)
  {
    this.service = service;
  }
  
  [HttpPost]
  public async Task<ApiResponse> LoginAsync(string Account, String Password) =>
      await service.LoginAsync(Account, Password);

  [HttpPost]
  public async Task<ApiResponse> RegsiterAsync([FromBody] User user) => await service.RegisterAsync(user);

}

