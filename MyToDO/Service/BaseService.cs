using MyToDo.Shared;
using MyToDo.Shared.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDO.Service
{
  public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
  {
    private readonly HttpRestClient client;
    private readonly string serviceName;

    public BaseService(HttpRestClient client, string serviceName)
    {
      this.client = client;
      this.serviceName = serviceName;
    }

    public async Task<ApiResponse<TEntity>> AddAsync(TEntity entity)
    {
      BaseRequest req = new BaseRequest();
      req.Method = RestSharp.Method.POST;
      req.Route = $"api/{serviceName}/Add";
      req.Paramster = entity;
      var resp = await client.ExecuteAsync<TEntity>(req);
      return resp;
    }

    public async Task<ApiResponse> DeleteAsync(int id)
    {
      BaseRequest req = new BaseRequest();
      req.Method = RestSharp.Method.DELETE;
      req.Route = $"api/{serviceName}/Delete?id=${id}";
      var resp = await client.ExecuteAsync(req);
      return resp;
    }

    public async Task<ApiResponse<TEntity>> UpdateAsync(TEntity entity)
    {
      BaseRequest req = new BaseRequest();
      req.Method = RestSharp.Method.POST;
      req.Route = $"api/{serviceName}/Update";
      req.Paramster = entity;
      var resp = await client.ExecuteAsync<TEntity>(req);
      return resp;
    }

    public async Task<ApiResponse<PagedList<TEntity>>> GetAllAsync(QueryParameter parameter)
    {
      BaseRequest req = new BaseRequest();
      req.Method = RestSharp.Method.GET;
      req.Route = $"api/{serviceName}/GetAll?pageIndex={parameter.PageIndex}" +
        $"&pageSize={parameter.PageSize}&search={parameter.Search}";
      var resp = await client.ExecuteAsync<PagedList<TEntity>>(req);
      return resp;
    }

    public async Task<ApiResponse<TEntity>> GetAsync(int id)
    {
      BaseRequest req = new BaseRequest();
      req.Method = RestSharp.Method.GET;
      req.Route = $"api/{serviceName}/Get?id=${id}";
      var resp = await client.ExecuteAsync<TEntity>(req);
      return resp;
    }
  
  }
}
