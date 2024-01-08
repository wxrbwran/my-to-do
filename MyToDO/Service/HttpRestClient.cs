using MyToDo.Shared;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDO.Service
{
  public class HttpRestClient
  {
    private readonly string apiUrl;
    protected readonly RestClient client;

    public HttpRestClient(string apiUrl)
    {
      this.apiUrl = apiUrl;
      client = new RestClient();
    }

    public async Task<ApiResponse> ExecuteAsync(BaseRequest baseRequest)
    {
      RestRequest request = new RestRequest(baseRequest.Method);
      request.AddHeader("Content-Type", baseRequest.ContentType);
      if(baseRequest.Paramster != null)
      {
        request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Paramster), ParameterType.RequestBody);
      }
      client.BaseUrl = (new Uri(apiUrl + baseRequest.Route));
      IRestResponse restResponse = await client.ExecuteAsync(request);

      return JsonConvert.DeserializeObject<ApiResponse>(restResponse.Content);
    }

    public async Task<ApiResponse<T>> ExecuteAsync<T>(BaseRequest baseRequest)
    {
      RestRequest request = new RestRequest(baseRequest.Method);
      request.AddHeader("Content-Type", baseRequest.ContentType);
      if (baseRequest.Paramster != null)
      {
        request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Paramster), ParameterType.RequestBody);
      }
      client.BaseUrl = (new Uri(apiUrl + baseRequest.Route));

      IRestResponse restResponse = await client.ExecuteAsync(request);

      return JsonConvert.DeserializeObject<ApiResponse<T>>(restResponse.Content);
    }
  }
}
