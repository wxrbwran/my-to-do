using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using MyToDO.Common;
using MyToDO.Extensions;
using MyToDO.Service;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System.Collections.ObjectModel;

namespace MyToDO.ViewModels
{
	public class ToDoViewModel : NavigationViewModel
	{
		private readonly IToDoService service;
		private readonly IDialogHostService dialogHostService;
		public DelegateCommand<ToDoDto> SelectedCommand { get; private set; }
		public DelegateCommand<ToDoDto> DeleteCommand { get; private set; }
		public DelegateCommand<string> ExecuteCommand { get; private set; }

		public ToDoViewModel(IToDoService service, IContainerProvider provider) : base(provider)
		{
			TodoDtos = new ObservableCollection<ToDoDto>();
			ExecuteCommand = new DelegateCommand<string>(Execute);
			this.service = service;
			dialogHostService = provider.Resolve<IDialogHostService>();
			SelectedCommand = new DelegateCommand<ToDoDto>(HandleSelectToDoItem);
			DeleteCommand = new DelegateCommand<ToDoDto>(HandleDeleteToDoItem);
		}

		#region 属性
		private string isEmptyData;
		public string IsEmptyData
		{
			get { return isEmptyData; }
			set { isEmptyData = value; RaisePropertyChanged(); }
		}

		private int selectedIndex;
		public int SelectedIndex
		{
			get { return selectedIndex; }
			set { selectedIndex = value; RaisePropertyChanged(); }
		}


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

		private ObservableCollection<ToDoDto> todoDtos;
		public ObservableCollection<ToDoDto> TodoDtos
		{
			get { return todoDtos; }
			set { todoDtos = value; RaisePropertyChanged(); }
		}
		#endregion 属性

		#region simple command
		private void Execute(string command)
		{
			switch (command)
			{
				case "ShowAddToDo":
					ShowAddToDo(); break;
				case "SearchToDo":
					SearchToDo(); break;
				case "SaveEditToDo":
					SaveEditToDo(); break;
			}
		}

		private void SearchToDo()
		{
			GetDataAsync();
		}
		private void ShowAddToDo()
		{
			IsRightDrawerOpen = true;
			CurrentDto = new ToDoDto();
		}

		private async void SaveEditToDo()
		{
			try
			{
				if (string.IsNullOrWhiteSpace(currentDto.Title) || string.IsNullOrWhiteSpace(currentDto.Content))
				{
					return;
				}
				UpdateLoading(true);
				if (currentDto.Id > 0)
				// 编辑
				{
					var resp = await service.UpdateAsync(currentDto);
					if (resp.Status)
					{
						GetDataAsync();
					}
				}
				else
				{
					//新增
					var resp = await service.AddAsync(currentDto);
					if (resp.Status)
					{
						GetDataAsync();
					}
				}

			}
			catch (Exception ex)
			{

			}
			finally
			{
				UpdateLoading(false);
				IsRightDrawerOpen = false;
			}
		}

		#endregion simple command

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

			}
			catch (Exception ex) { }
			finally { UpdateLoading(false); }
		}

		private async void HandleDeleteToDoItem(ToDoDto dto)
		{
			try
			{
				var result = await dialogHostService.Question(title: "危险操作", content:"删除后不可找回，确定删除吗？");
				if (result.Result != Prism.Services.Dialogs.ButtonResult.OK) return;
				await service.DeleteAsync(dto.Id);
				GetDataAsync();
			}
			catch (Exception ex) { }
		}

		/// <summary>
		/// 获取数据
		/// </summary>
		public async void GetDataAsync()
		{
			UpdateLoading(true);
			int? Status = SelectedIndex == 0 ? null : SelectedIndex == 2 ? 1 : 0;
			var resp = await service.GetFilterAllAsync(new ToDoQueryParameter()
			{
				PageIndex = 0,
				PageSize = 100,
				Search = Search,
				Status = Status
			});
			if (resp.Status)
			{
				todoDtos.Clear();
				foreach (var item in resp.Result.Items)
				{
					TodoDtos.Add(item);
				}
				IsEmptyData = (todoDtos.Count == 0) ? "Visiable" : "Hidden";
			}
			UpdateLoading(false);
		}

		public override void OnNavigatedTo(NavigationContext navigationContext)
		{
			base.OnNavigatedTo(navigationContext);
			if (navigationContext.Parameters.ContainsKey("Status"))
			{
				SelectedIndex = navigationContext.Parameters.GetValue<int>("Status");
			}
			else
			{
				SelectedIndex = 0;
			}
			GetDataAsync();
		}
	}
}
