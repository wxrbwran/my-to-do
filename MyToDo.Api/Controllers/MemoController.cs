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
public class MemoController: ControllerBase
{
  private readonly IMemoService service;

  public MemoController(IMemoService service)
  {
    this.service = service;
  }

  [HttpGet]
  public async Task<ApiResponse> GetAsync(int id) => await service.GetByIdAsync(id);

  [HttpGet]
  public async Task<ApiResponse> GetAllAsync([FromQuery] QueryParameter parameter) => await service.GetAllAsync(parameter);
  
  [HttpPost]
  public async Task<ApiResponse> AddAsync([FromBody] MemoDto dto) => await service.AddAsync(dto);

  [HttpPost]
  public async Task<ApiResponse> UpdateAsync([FromBody] MemoDto dto) => await service.UpdateAsync(dto);

  [HttpDelete]
  public async Task<ApiResponse> DeleteAsync(int id) => await service.DeleteByIdAsync(id);
 
}

