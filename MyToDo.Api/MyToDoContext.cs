using Microsoft.EntityFrameworkCore;
using MyToDo.Api.Models;

namespace MyToDo.Api
{
    public class MyToDoContext : DbContext
    {

        public MyToDoContext(DbContextOptions<MyToDoContext> options) : base(options)
        {

        }

      //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      //{
      //  //base.OnConfiguring(optionsBuilder);
      //  //optionsBuilder.UseSqlite("Data Sourse=MyToDo.db");

      //}

      public DbSet<User> User { get; set; }
      public DbSet<ToDo> ToDo { get; set; }
      public DbSet<Memo> Memo { get; set; }
      //public DbSet<Book> Book { get; set; }
  }
}
