using Microsoft.AspNetCore.Mvc;
using MyToDo.Api.Models;
using MyToDo.Api.Services;
using MyToDo.Api.UnitOfWork;
using MyToDo.Shared;
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
  public async Task<ApiResponse> LoginAsync([FromBody] UserDto dto) =>
      await service.LoginAsync(dto);

  [HttpPost]
  public async Task<ApiResponse> RegisterAsync([FromBody] User user) =>
    await service.RegisterAsync(user);

}

