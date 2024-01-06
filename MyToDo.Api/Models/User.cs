namespace MyToDo.Api.Models
{
  public class User : BaseEntity
  {
    public string Account { get; set; }

    public string Password { get; set; }

    public string Username { get; set; }
  }
}
