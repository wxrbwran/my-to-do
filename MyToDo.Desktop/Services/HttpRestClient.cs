using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using MyToDo.Shared;

namespace MyToDo.Desktop.Services
{
    public class HttpRestClient
    {
        private readonly HttpClient _client;
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public HttpRestClient(string baseUrl)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/")
            };
        }

        public async Task<ApiResponse<T>> GetAsync<T>(string route)
        {
            try
            {
                var response = await _client.GetAsync(route.TrimStart('/'));
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<T>>(JsonOptions);
                    return result ?? new ApiResponse<T> { Status = false, Message = "响应内容为空" };
                }
                return new ApiResponse<T> { Status = false, Message = $"HTTP 错误: {(int)response.StatusCode}" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<T> { Status = false, Message = $"网络连接失败: {ex.Message}" };
            }
        }

        public async Task<ApiResponse> GetAsync(string route)
        {
            try
            {
                var response = await _client.GetAsync(route.TrimStart('/'));
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse>(JsonOptions);
                    return result ?? new ApiResponse("响应内容为空", false);
                }
                return new ApiResponse($"HTTP 错误: {(int)response.StatusCode}", false);
            }
            catch (Exception ex)
            {
                return new ApiResponse($"网络连接失败: {ex.Message}", false);
            }
        }

        public async Task<ApiResponse<T>> PostAsync<T>(string route, object body)
        {
            try
            {
                var response = await _client.PostAsJsonAsync(route.TrimStart('/'), body, JsonOptions);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<T>>(JsonOptions);
                    return result ?? new ApiResponse<T> { Status = false, Message = "响应内容为空" };
                }
                return new ApiResponse<T> { Status = false, Message = $"HTTP 错误: {(int)response.StatusCode}" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<T> { Status = false, Message = $"网络连接失败: {ex.Message}" };
            }
        }

        public async Task<ApiResponse> PostAsync(string route, object body)
        {
            try
            {
                var response = await _client.PostAsJsonAsync(route.TrimStart('/'), body, JsonOptions);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse>(JsonOptions);
                    return result ?? new ApiResponse("响应内容为空", false);
                }
                return new ApiResponse($"HTTP 错误: {(int)response.StatusCode}", false);
            }
            catch (Exception ex)
            {
                return new ApiResponse($"网络连接失败: {ex.Message}", false);
            }
        }

        public async Task<ApiResponse> DeleteAsync(string route)
        {
            try
            {
                var response = await _client.DeleteAsync(route.TrimStart('/'));
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse>(JsonOptions);
                    return result ?? new ApiResponse("响应内容为空", false);
                }
                return new ApiResponse($"HTTP 错误: {(int)response.StatusCode}", false);
            }
            catch (Exception ex)
            {
                return new ApiResponse($"网络连接失败: {ex.Message}", false);
            }
        }
    }
}
