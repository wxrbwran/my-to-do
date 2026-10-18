using System.Threading.Tasks;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

namespace MyToDo.Desktop.Services
{
    public interface IToDoService
    {
        Task<ApiResponse<SummaryDto>> GetSummaryAsync();
        Task<ApiResponse<PagedList<ToDoDto>>> GetAllFilterAsync(ToDoQueryParameter parameter);
        Task<ApiResponse<ToDoDto>> AddAsync(ToDoDto model);
        Task<ApiResponse<ToDoDto>> UpdateAsync(ToDoDto model);
        Task<ApiResponse> DeleteAsync(int id);
    }

    public class ToDoService : IToDoService
    {
        private readonly HttpRestClient _client;

        public ToDoService(HttpRestClient client)
        {
            _client = client;
        }

        public Task<ApiResponse<SummaryDto>> GetSummaryAsync()
        {
            return _client.GetAsync<SummaryDto>("api/ToDo/Summary");
        }

        public Task<ApiResponse<PagedList<ToDoDto>>> GetAllFilterAsync(ToDoQueryParameter parameter)
        {
            string url = $"api/ToDo/GetFilterAll?pageIndex={parameter.PageIndex}&pageSize={parameter.PageSize}&search={parameter.Search}&status={parameter.Status}";
            return _client.GetAsync<PagedList<ToDoDto>>(url);
        }

        public Task<ApiResponse<ToDoDto>> AddAsync(ToDoDto model)
        {
            return _client.PostAsync<ToDoDto>("api/ToDo/Add", model);
        }

        public Task<ApiResponse<ToDoDto>> UpdateAsync(ToDoDto model)
        {
            return _client.PostAsync<ToDoDto>("api/ToDo/Update", model);
        }

        public Task<ApiResponse> DeleteAsync(int id)
        {
            return _client.DeleteAsync($"api/ToDo/Delete?id={id}");
        }
    }

    public interface IMemoService
    {
        Task<ApiResponse<PagedList<MemoDto>>> GetAllAsync(QueryParameter parameter);
        Task<ApiResponse<MemoDto>> AddAsync(MemoDto model);
        Task<ApiResponse<MemoDto>> UpdateAsync(MemoDto model);
        Task<ApiResponse> DeleteAsync(int id);
    }

    public class MemoService : IMemoService
    {
        private readonly HttpRestClient _client;

        public MemoService(HttpRestClient client)
        {
            _client = client;
        }

        public Task<ApiResponse<PagedList<MemoDto>>> GetAllAsync(QueryParameter parameter)
        {
            string url = $"api/Memo/GetAll?pageIndex={parameter.PageIndex}&pageSize={parameter.PageSize}&search={parameter.Search}";
            return _client.GetAsync<PagedList<MemoDto>>(url);
        }

        public Task<ApiResponse<MemoDto>> AddAsync(MemoDto model)
        {
            return _client.PostAsync<MemoDto>("api/Memo/Add", model);
        }

        public Task<ApiResponse<MemoDto>> UpdateAsync(MemoDto model)
        {
            return _client.PostAsync<MemoDto>("api/Memo/Update", model);
        }

        public Task<ApiResponse> DeleteAsync(int id)
        {
            return _client.DeleteAsync($"api/Memo/Delete?id={id}");
        }
    }

    public interface IAuthService
    {
        Task<ApiResponse<UserDto>> LoginAsync(UserDto model);
        Task<ApiResponse> RegisterAsync(UserDto model);
    }

    public class AuthService : IAuthService
    {
        private readonly HttpRestClient _client;

        public AuthService(HttpRestClient client)
        {
            _client = client;
        }

        public Task<ApiResponse<UserDto>> LoginAsync(UserDto model)
        {
            return _client.PostAsync<UserDto>("api/Auth/Login", model);
        }

        public Task<ApiResponse> RegisterAsync(UserDto model)
        {
            return _client.PostAsync("api/Auth/Register", model);
        }
    }
}
