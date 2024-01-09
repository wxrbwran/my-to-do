using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDO.Service
{
  public interface IToDoService: IBaseService<ToDoDto>
  {
		Task<ApiResponse<PagedList<ToDoDto>>> GetFilterAllAsync(ToDoQueryParameter parameter);
	}
}
