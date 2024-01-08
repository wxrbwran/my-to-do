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
      SelectedCommand = new DelegateCommand<ToDoDto>(HandleSelectToDoItem);
		}

		public DelegateCommand<ToDoDto> SelectedCommand { get; private set; }

    private string search;

    public string Search
		{
      get { return search; }
      set { search = value; RaisePropertyChanged(); }
    }


    private bool isRightDrawerOpen;

    public bool IsRightDrawerOpen
    {
      get { return isRightDrawerOpen; }
      set { isRightDrawerOpen = value; RaisePropertyChanged(); }
    }

    /// <summary>
    /// 编辑选中/新增时对象
    /// </summary>
    private ToDoDto currentDto;
    public ToDoDto CurrentDto
		{
      get { return currentDto; }
      set { currentDto = value; RaisePropertyChanged(); }
    }

    private void ShowAddToDo()
    {
      IsRightDrawerOpen = true;
    }

		private async void HandleSelectToDoItem(ToDoDto dto)
		{
      try
      {
				UpdateLoading(true);
				IsRightDrawerOpen = true;
				var resp = await service.GetFirstOfDefaultAsync(dto.Id);
				if (resp.Status)
				{

					CurrentDto = resp.Result;
					IsRightDrawerOpen = true;
				}
				
			} catch (Exception ex) { }
      finally { UpdateLoading(false);  }
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
