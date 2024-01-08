using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using MyToDO.Service;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyToDO.ViewModels
{
  public class ToDoViewModel:NavigationViewModel
  {
    private readonly IToDoService service;

    public ToDoViewModel(IToDoService service, IContainerProvider provider): base(provider) 
		{
			TodoDtos = new ObservableCollection<ToDoDto>();
      ShowAddToDoCommand = new DelegateCommand(ShowAddToDo);
      this.service = service;
      //HandleGetToDoLists();

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

    /// <summary>
    /// 获取数据
    /// </summary>
    public async void GetDataAsync()
    {
      UpdateLoading(true);
      var resp = await service.GetAllAsync(new QueryParameter() {
        PageIndex = 0,
        PageSize = 100,
      });
      if(resp.Status)
      {
        foreach (var item in resp.Result.Items)
        {
          TodoDtos.Add(item);
        }
      }
			UpdateLoading(false);
		}

		public override void OnNavigatedTo(NavigationContext navigationContext)
		{
			base.OnNavigatedTo(navigationContext);
      GetDataAsync();
		}
	}
}
