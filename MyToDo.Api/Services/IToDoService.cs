using MyToDo.Api.Models;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

namespace MyToDo.Api.Services
{
  public interface IToDoService: IBaseService<ToDoDto>
  {
    public Task<ApiResponse<PagedList<ToDoDto>>> GetAllFilterAsync(ToDoQueryParameter parameter);
  }
}
