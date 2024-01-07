using AutoMapper;
using MyToDo.Api.Models;
using MyToDo.Api.UnitOfWork;
using MyToDo.Shared.Dtos;

namespace MyToDo.Api.Services
{
  public class ToDoService : IToDoService
  {
    private readonly IUnitOfWork worker;
    private readonly IMapper mapper;
    private IRepository<ToDo> toDoRepo;

    public ToDoService(IUnitOfWork unitOfWork, IMapper mapper)
    {
      this.worker = unitOfWork;
      this.mapper = mapper;
      this.toDoRepo = unitOfWork.GetRepository<ToDo>();
    }

    public async Task<ApiResponse> AddAsync(ToDoDto model)
    {
      try
      {
        ToDo toDo = mapper.Map<ToDo>(model);
        await toDoRepo.InsertAsync(toDo);
        int count = await worker.SaveChangesAsync();
        if (count > 0)
        {
          return new ApiResponse(true, toDo);
        }
        return new ApiResponse("新建待办失败");
      } catch (Exception ex)
      {
        return new ApiResponse(ex.Message);
      }
    }

    public async Task<ApiResponse> DeleteByIdAsync(int id)
    {
      try
      {
        ToDo toDo = await toDoRepo.FindAsync(id);
        toDoRepo.Delete(toDo);
        int count = await worker.SaveChangesAsync();
        if (count > 0)
        {
          return new ApiResponse(true, "");
        }
        return new ApiResponse("删除待办失败");
      }
      catch (Exception ex)
      {
        return new ApiResponse(ex.Message);
      }
    }

    public async Task<ApiResponse> UpdateAsync(ToDoDto model)
    {
      try
      {
        ToDo oldToDo = await toDoRepo.GetFirstOrDefaultAsync(predicate: t => t.Id.Equals(model.Id));
        oldToDo.Title = model.Title;
        oldToDo.Content = model.Content;
        oldToDo.Status = model.Status;
        oldToDo.UpdatedAt = DateTime.Now;

        toDoRepo.Update(oldToDo);
        int count = await worker.SaveChangesAsync();
        if (count > 0)
        {
          return new ApiResponse(true, oldToDo);
        }
        return new ApiResponse("更新待办失败");
      }
      catch (Exception ex)
      {
        return new ApiResponse(ex.Message);
      }
    }


    public async Task<ApiResponse> GetAllAsync()
    {
      try
      {
        IList<ToDo> toDos = await toDoRepo.GetAllAsync();
        return new ApiResponse(true, toDos);
      }
      catch (Exception ex)
      {
        return new ApiResponse(ex.Message);
      }
    }

    public async Task<ApiResponse> GetByIdAsync(int id)
    {
      try
      {
        ToDo toDo = await toDoRepo.GetFirstOrDefaultAsync(predicate: t => t.Id.Equals(id));
        if (toDo != null)
        {
          return new ApiResponse(true, toDo);
        }
        return new ApiResponse("查找待办失败");
      }
      catch (Exception ex)
      {
        return new ApiResponse(ex.Message);
      }
    }

   
  }
}
