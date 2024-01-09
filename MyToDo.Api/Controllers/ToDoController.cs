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
public class ToDoController: ControllerBase
{
  private readonly IToDoService toDoService;

  public ToDoController(IToDoService toDoService)
  {
    this.toDoService = toDoService;
  }

  [HttpGet]
  public async Task<ApiResponse> GetAsync(int id) => await toDoService.GetByIdAsync(id);

  [HttpGet]
  public async Task<ApiResponse> GetAllAsync([FromQuery] QueryParameter parameter) => await toDoService.GetAllAsync(parameter);

	[HttpGet]
	public async Task<ApiResponse> GetFilterAllAsync([FromQuery] ToDoQueryParameter parameter) => await toDoService.GetFilterAllAsync(parameter);

	[HttpPost]
  public async Task<ApiResponse> AddAsync([FromBody] ToDoDto todo) => await toDoService.AddAsync(todo);

  [HttpPost]
  public async Task<ApiResponse> UpdateAsync([FromBody] ToDoDto todo) => await toDoService.UpdateAsync(todo);

  [HttpDelete]
  public async Task<ApiResponse> DeleteAsync(int id) => await toDoService.DeleteByIdAsync(id);
 
}

