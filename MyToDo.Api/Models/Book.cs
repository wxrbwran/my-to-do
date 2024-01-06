namespace MyToDo.Api.Models
{
  public class Book
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public int Desc { get; set; }
    public DateTime PublishedOn { get; set; }
    public string Publisher { get; set; }

    public decimal Price { get; set; }
    public string ImageUrl { get; set; }


  }
}
