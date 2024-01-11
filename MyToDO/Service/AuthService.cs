using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDO.Service
{
	public class AuthService : IAuthService
	{
		private readonly HttpRestClient client;

		public AuthService(HttpRestClient client)
		{
			this.client = client;
		}

		public async Task<ApiResponse<UserDto>> LoginAsync(UserDto dto)
		{
			BaseRequest request = new BaseRequest();
			request.Method = RestSharp.Method.POST;
			request.Route = "api/Auth/Login";
			request.Parameter = dto;
			return await client.ExecuteAsync<UserDto>(request);
		}

		public async Task<ApiResponse> RegisterAsync(UserDto dto)
		{
			BaseRequest request = new BaseRequest();
			request.Method = RestSharp.Method.POST;
			request.Route = "api/Auth/Register";
			request.Parameter = dto;
			return await client.ExecuteAsync(request);
		}
		
	}
}
