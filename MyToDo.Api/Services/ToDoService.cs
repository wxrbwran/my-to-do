using AutoMapper;
using MyToDo.Api.Models;
using MyToDo.Api.UnitOfWork;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Reflection.Metadata;

namespace MyToDo.Api.Services
{
  public class ToDoService : IToDoService
  {
    private readonly IUnitOfWork worker;
    private readonly IMapper mapper;
    private IRepository<ToDo> repo;
    private IRepository<Memo> memoRepo;

		public ToDoService(IUnitOfWork unitOfWork, IMapper mapper)
    {
      this.worker = unitOfWork;
      this.mapper = mapper;
      this.repo = unitOfWork.GetRepository<ToDo>();
      this.memoRepo = unitOfWork.GetRepository<Memo>();
		}

    public async Task<ApiResponse> AddAsync(ToDoDto model)
    {
      try
      {
        ToDo toDo = mapper.Map<ToDo>(model);
        await repo.InsertAsync(toDo);
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
        ToDo toDo = await repo.FindAsync(id);
        repo.Delete(toDo);
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
        ToDo oldToDo = await repo.GetFirstOrDefaultAsync(predicate: t => t.Id.Equals(model.Id));
        oldToDo.Title = model.Title;
        oldToDo.Content = model.Content;
        oldToDo.Status = model.Status;
        oldToDo.UpdatedAt = DateTime.Now;

        repo.Update(oldToDo);
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

    public async Task<ApiResponse> GetByIdAsync(int id)
    {
      try
      {
        ToDo toDo = await repo.GetFirstOrDefaultAsync(predicate: t => t.Id.Equals(id));
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

    public async Task<ApiResponse> GetAllAsync(QueryParameter parameter)
    {
      try
      {
        var todos = await repo.GetPagedListAsync(predicate:
                  x => string.IsNullOrWhiteSpace(parameter.Search) ? true : x.Title.Contains(parameter.Search),
									pageIndex: parameter.PageIndex,
                  pageSize: parameter.PageSize,
                  orderBy: source => source.OrderByDescending(t => t.CreatedAt));
        return new ApiResponse(true, todos);
      }
      catch (Exception ex)
      {
        return new ApiResponse(ex.Message);
      }
    }

		public async Task<ApiResponse> GetFilterAllAsync(ToDoQueryParameter parameter)
		{
			try
			{
				var todos = await repo.GetPagedListAsync(predicate:
				x =>
					(string.IsNullOrWhiteSpace(parameter.Search) ? true : x.Title.Contains(parameter.Search))
					&& (parameter.Status == null ? true : x.Status.Equals(parameter.Status)),
				pageIndex: parameter.PageIndex,
									pageSize: parameter.PageSize,
									orderBy: source => source.OrderByDescending(t => t.CreatedAt));
				return new ApiResponse(true, todos);
			}
			catch (Exception ex)
			{
				return new ApiResponse(ex.Message);
			}
		}

		public async Task<ApiResponse> Summary()
		{
			try
			{
				var todos = await repo.GetAllAsync(predicate:
				t => t.DeletedAt == null,
				orderBy: (source) => source.OrderByDescending(t => t.CreatedAt)
			);
				var memos = await memoRepo.GetAllAsync(predicate:
					t => t.DeletedAt == null,
					orderBy: (source) => source.OrderByDescending(t => t.CreatedAt)
				);
				SummaryDto summaryDto = new SummaryDto();
				summaryDto.ToDoList = new ObservableCollection<ToDoDto>(
					todos.Where(t => t.Status == 0)
					.Select(t => mapper.Map<ToDoDto>(t))
				);
				summaryDto.TodoCount = todos.Count;
				summaryDto.CompletedToDoCount = todos.Where(t => t.Status.Equals(1)).Count();
				summaryDto.CompletedToDoRatio = (summaryDto.CompletedToDoCount / (double)summaryDto.TodoCount).ToString("0%");
				summaryDto.MemoList = new ObservableCollection<MemoDto>(
					memos.Select(m => mapper.Map<MemoDto>(m))
				);
				summaryDto.MemoCount = memos.Count;

				return new ApiResponse(true, summaryDto);
			}
			catch (Exception ex)
			{
				return new ApiResponse(false, ex.Message);
			}
			
		}
	}
}
