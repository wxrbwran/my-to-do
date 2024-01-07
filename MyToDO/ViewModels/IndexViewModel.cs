using MyToDo.Shared.Dtos;
using MyToDO.Common.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDO.ViewModels
{
  public class IndexViewModel: BindableBase
  {
    public IndexViewModel() {

			TaskBars = new ObservableCollection<IndexTaskBar>();
			CreateTaskBars();
      CreateTestData();
		}

  
    private ObservableCollection<IndexTaskBar> taskBars;

		public ObservableCollection<IndexTaskBar> TaskBars
		{
			get { return taskBars; }
			set { taskBars = value; RaisePropertyChanged(); }
		}

    private ObservableCollection<ToDoDto> todoDtos;

    public ObservableCollection<ToDoDto> ToDoDtos
    {
      get { return todoDtos; }
      set { todoDtos = value; RaisePropertyChanged(); }
    }

    private ObservableCollection<MemoDto> memoDtos;

    public ObservableCollection<MemoDto> MemoDtos
    {
      get { return memoDtos; }
      set { memoDtos = value; RaisePropertyChanged(); }
    }


    private void CreateTaskBars()
    {
      TaskBars.Add(new IndexTaskBar() { Icon = "ClockFast", Title = "汇总", Content = "9", Color = "#FF0CA0FF", Target = "" });
      TaskBars.Add(new IndexTaskBar() { Icon = "ClockCheckOutline", Title = "已完成", Content = "8", Color = "#FF1ECA3A", Target = "" });
      TaskBars.Add(new IndexTaskBar() { Icon = "ChartLineVariant", Title = "完成比例", Content = "89%", Color = "#FF02C6DC", Target = "" });
      TaskBars.Add(new IndexTaskBar() { Icon = "PlaylistStar", Title = "备忘录", Content = "4", Color = "#FFFFA000", Target = "" });
    }

    private void CreateTestData()
    {
      ToDoDtos = new ObservableCollection<ToDoDto>();
      MemoDtos = new ObservableCollection<MemoDto>();

      for (int i = 0; i < 10; i++)
      {
        ToDoDtos.Add(new ToDoDto() { Title=$"代办 {i}", Id=i, Content=$"正在做代办: {i}", CreatedAt=DateTime.Now,  });
        MemoDtos.Add(new MemoDto() { Title = $"备忘 {i}", Id = i, Content = $"备忘内容: {i}", CreatedAt = DateTime.Now, });
      }
    }

  }
}
