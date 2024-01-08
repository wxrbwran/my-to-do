
namespace MyToDo.Shared
{
  public class ApiResponse
  {
    public ApiResponse()
    {
    }

    public ApiResponse(bool status, object result)
    {
      this.Status = status;
      this.Result = result;
    }
    public ApiResponse(string message, bool status = false)
    {
      this.Status = status;
      this.Message = message;
      this.Result = null;
    }
    public string Message { get; set; }

    public bool Status { get; set; }

    public object Result { get; set; }
  }

  public class ApiResponse<T>
  {
   
    public string Message { get; set; }

    public bool Status { get; set; }

    public T Result { get; set; }
  }
}
