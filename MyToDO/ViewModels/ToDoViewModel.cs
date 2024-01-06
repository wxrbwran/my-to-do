using MyToDO.Common.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyToDO.ViewModels
{
  public class ToDoViewModel:BindableBase
  {
    public ToDoViewModel()
		{
			TodoDtos = new ObservableCollection<ToDoDto>();
      ShowAddToDoCommand = new DelegateCommand(ShowAddToDo);
			CreateTestDtos();
		}

    private bool isRightDrawerOpen;

    public bool IsRightDrawerOpen
    {
      get { return isRightDrawerOpen; }
      set { isRightDrawerOpen = value; RaisePropertyChanged(); }
    }


    private void ShowAddToDo()
    {
      IsRightDrawerOpen = true;
    }

    public DelegateCommand ShowAddToDoCommand { get; private set; }

    private ObservableCollection<ToDoDto> todoDtos;

		public ObservableCollection<ToDoDto> TodoDtos
		{
			get { return todoDtos; }
			set { todoDtos = value; RaisePropertyChanged();  }
		}

    void CreateTestDtos()
    {
      for (int i = 0; i < 20; i++)
      {
        TodoDtos.Add(new ToDoDto() {  Id = i, Title=$"待办{i}", Content=$"待办内容{i}", CreatedAt=DateTime.Now });
      }
    }
  }
}
