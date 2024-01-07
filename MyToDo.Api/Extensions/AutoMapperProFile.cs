using AutoMapper;
using MyToDo.Api.Models;
using MyToDo.Shared.Dtos;

namespace MyToDo.Api.Extensions
{
  public class AutoMapperProFile: MapperConfigurationExpression
  {
    public AutoMapperProFile()
    {
      CreateMap<ToDo, ToDoDto>().ReverseMap();     
      CreateMap<Memo, MemoDto>().ReverseMap();
    }
  }
}
