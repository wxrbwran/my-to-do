using MyToDo.Api.Models;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;

namespace MyToDo.Api.Services
{
  public interface IAuthService
  {

    public Task<ApiResponse> LoginAsync(UserDto dto);

    public Task<ApiResponse> RegisterAsync(User user);
  }
}
