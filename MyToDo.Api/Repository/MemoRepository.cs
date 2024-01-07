using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyToDo.Api.Models;
using MyToDo.Api.UnitOfWork;

namespace MyToDo.Api.Repository;

//public interface ToDoRepository
//{
//  Task<bool> Add(ToDo toDo);

//  Task<bool> Update(ToDo todo);

//  Task<bool> Delete(int id);
//}

public class MemoRepository : Repository<Memo>, IRepository<Memo>
{
  public MemoRepository(MyToDoContext dbContext) : base(dbContext)
  {

  }
}

