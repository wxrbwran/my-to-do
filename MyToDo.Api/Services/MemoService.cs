using AutoMapper;
using MyToDo.Api.Models;
using MyToDo.Api.UnitOfWork;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

namespace MyToDo.Api.Services
{
  public class MemoService : IMemoService
  {
    private readonly IUnitOfWork worker;
    private readonly IMapper mapper;
    private IRepository<Memo> repo;

    public MemoService(IUnitOfWork unitOfWork, IMapper mapper)
    {
      this.worker = unitOfWork;
      this.mapper = mapper;
      this.repo = unitOfWork.GetRepository<Memo>();
    }

    public async Task<ApiResponse> AddAsync(MemoDto model)
    {
      try
      {
        Memo memo = mapper.Map<Memo>(model);
        await repo.InsertAsync(memo);
        int count = await worker.SaveChangesAsync();
        if (count > 0)
        {
          return new ApiResponse(true, memo);
        }
        return new ApiResponse("新建备忘录失败");
      } catch (Exception ex)
      {
        return new ApiResponse(ex.Message);
      }
    }

    public async Task<ApiResponse> DeleteByIdAsync(int id)
    {
      try
      {
        var memo = await repo.FindAsync(id);
        repo.Delete(memo);
        int count = await worker.SaveChangesAsync();
        if (count > 0)
        {
          return new ApiResponse(true, "");
        }
        return new ApiResponse("删除备忘录失败");
      }
      catch (Exception ex)
      {
        return new ApiResponse(ex.Message);
      }
    }

    public async Task<ApiResponse> UpdateAsync(MemoDto model)
    {
      try
      {
        var oldMemo = await repo.GetFirstOrDefaultAsync(predicate: t => t.Id.Equals(model.Id));
        oldMemo.Title = model.Title;
        oldMemo.Content = model.Content;
        oldMemo.UpdatedAt = DateTime.Now;

        repo.Update(oldMemo);
        int count = await worker.SaveChangesAsync();
        if (count > 0)
        {
          return new ApiResponse(true, oldMemo);
        }
        return new ApiResponse("更新备忘录失败");
      }
      catch (Exception ex)
      {
        return new ApiResponse(ex.Message);
      }
    }


    public async Task<ApiResponse> GetAllAsync(QueryParameter parameter)
    {
      try
      {
      
        var memos = await repo.GetPagedListAsync(predicate:
                  x => string.IsNullOrWhiteSpace(parameter.Search) ? true : x.Title.Contains(parameter.Search),
                  pageIndex: parameter.PageIndex,
                  pageSize: parameter.PageSize,
                  orderBy: source => source.OrderByDescending(t => t.CreatedAt));
        return new ApiResponse(true, memos);
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
        var memo = await repo.GetFirstOrDefaultAsync(predicate: t => t.Id.Equals(id));
        if (memo != null)
        {
          return new ApiResponse(true, memo);
        }
        return new ApiResponse("查找备忘录失败");
      }
      catch (Exception ex)
      {
        return new ApiResponse(ex.Message);
      }
    }

   
  }
}
