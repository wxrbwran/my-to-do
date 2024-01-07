using MyToDo.Shared.Dtos;
using MyToDO.Common.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDO.ViewModels
{
  public class MemoViewModel: BindableBase
  {
    public MemoViewModel()
    {
      MemoDtos = new ObservableCollection<MemoDto>();
      ShowAddMemoCommand = new DelegateCommand(ShowAddMemo);
      CreateTestDtos();
    }

    private bool isRightDrawerOpen;

    public bool IsRightDrawerOpen
    {
      get { return isRightDrawerOpen; }
      set { isRightDrawerOpen = value; RaisePropertyChanged(); }
    }


    private void ShowAddMemo()
    {
      IsRightDrawerOpen = true;
    }

    public DelegateCommand ShowAddMemoCommand { get; private set; }

    private ObservableCollection<MemoDto> memoDtos;

    public ObservableCollection<MemoDto> MemoDtos
    {
      get { return memoDtos; }
      set { memoDtos = value; RaisePropertyChanged(); }
    }

    void CreateTestDtos()
    {
      for (int i = 0; i < 20; i++)
      {
        memoDtos.Add(new MemoDto() { Id = i, Title = $"待办{i}", Content = $"待办内容{i}", CreatedAt = DateTime.Now });
      }
    }
  }
}
