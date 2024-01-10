using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

namespace MyToDO.Service
{
	public interface IToDoService : IBaseService<ToDoDto>
	{
		Task<ApiResponse<PagedList<ToDoDto>>> GetFilterAllAsync(ToDoQueryParameter parameter);
		Task<ApiResponse<SummaryDto>> SummaryAsync();
	}
}
