using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

namespace MyToDO.Service
{
	public class ToDoService : BaseService<ToDoDto>, IToDoService
	{
		private readonly HttpRestClient client;

		public ToDoService(HttpRestClient client) : base(client, "ToDo")
		{
			this.client = client;
		}

		public async Task<ApiResponse<PagedList<ToDoDto>>> GetFilterAllAsync(ToDoQueryParameter parameter)
		{
			BaseRequest request = new BaseRequest();
			request.Method = RestSharp.Method.GET;
			request.Route = $"api/ToDo/GetFilterAll?pageIndex={parameter.PageIndex}" +
					$"&pageSize={parameter.PageSize}" +
					$"&search={parameter.Search}" +
					$"&status={parameter.Status}";
			return await client.ExecuteAsync<PagedList<ToDoDto>>(request);
		}
	}
}
