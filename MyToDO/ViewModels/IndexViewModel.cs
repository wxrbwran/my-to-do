using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDO.Common;
using MyToDO.Common.Models;
using MyToDO.Service;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.Windows;

namespace MyToDO.ViewModels
{
	public class IndexViewModel : NavigationViewModel
	{
		public DelegateCommand<string> ExecuteCommand { get; private set; }
		public DelegateCommand<ToDoDto> EditToDoCommand { get; private set; }
		public DelegateCommand<MemoDto> EditMemoCommand { get; private set; }

		private readonly IToDoService toDoService;
		private readonly IMemoService memoService;
		private readonly IDialogHostService dialogService;

		public IndexViewModel(IContainerProvider provider, IDialogHostService dialogService) : base(provider)
		{

			TaskBars = new ObservableCollection<IndexTaskBar>();
			CreateTaskBars();

			ToDoDtos = new ObservableCollection<ToDoDto>();
			MemoDtos = new ObservableCollection<MemoDto>();
			ExecuteCommand = new DelegateCommand<string>(Execute);
			EditMemoCommand = new DelegateCommand<MemoDto>(AddEditMemo);
			EditToDoCommand = new DelegateCommand<ToDoDto>(AddEditToDo);
			this.toDoService = provider.Resolve<IToDoService>();
			this.memoService = provider.Resolve<IMemoService>();
			this.dialogService = dialogService;
		}


		#region 属性
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
		#endregion 属性

		private void Execute(string command)
		{
			switch (command)
			{
				case "AddToDo":
					AddEditToDo(null); break;
				case "AddMemo":
					AddEditMemo(null); break;
				
			}
		}

		private async void AddEditToDo(ToDoDto toDoDto)
		{
			DialogParameters dialogParameters = new DialogParameters();
			if (toDoDto != null)
			{
				dialogParameters.Add("Value", toDoDto);
			}
			var res = await dialogService.ShowDialog("AddToDoView", dialogParameters);
			if(res.Result == ButtonResult.OK)
			{
				ToDoDto dto = res.Parameters.GetValue<ToDoDto>("Value");
				if(dto.Id >0)
				{
					//编辑
					var apiResponse = await toDoService.UpdateAsync(dto);
					if (apiResponse.Status)
					{
						ToDoDto? oldDto = todoDtos.FirstOrDefault(t => t.Id.Equals(dto.Id));
						if (oldDto != null)
						{
							oldDto.Title = dto.Title;
							oldDto.Content = dto.Content;
						}
					}
				} else
				{
					// 新增
					var apiResponse = await toDoService.AddAsync(dto);
					if (apiResponse.Status)
					{
						todoDtos.Add(apiResponse.Result);
					}
				}
			}
		}

		private async void AddEditMemo(MemoDto memoDto)
		{
			DialogParameters dialogParameters = new DialogParameters();
			if (memoDto != null)
			{
				dialogParameters.Add("Value", memoDto);
			}
			var res = await dialogService.ShowDialog("AddMemoView", dialogParameters);
			if (res.Result == ButtonResult.OK)
			{
				MemoDto dto = res.Parameters.GetValue<MemoDto>("Value");
				if (dto.Id > 0)
				{
					//编辑
					var apiResponse = await memoService.UpdateAsync(dto);
					if (apiResponse.Status)
					{
						MemoDto? oldDto = memoDtos.FirstOrDefault(t => t.Id.Equals(dto.Id));
						if (oldDto != null)
						{
							oldDto.Title = dto.Title;
							oldDto.Content = dto.Content;
						}
					}

				}
				else
				{
					// 新增
					var apiResponse = await memoService.AddAsync(dto);
					if (apiResponse.Status)
					{
						memoDtos.Add(apiResponse.Result);
					}
				}
			}
		}

		private void CreateTaskBars()
		{
			TaskBars.Add(new IndexTaskBar() { Icon = "ClockFast", Title = "汇总", Content = "9", Color = "#FF0CA0FF", Target = "" });
			TaskBars.Add(new IndexTaskBar() { Icon = "ClockCheckOutline", Title = "已完成", Content = "8", Color = "#FF1ECA3A", Target = "" });
			TaskBars.Add(new IndexTaskBar() { Icon = "ChartLineVariant", Title = "完成比例", Content = "89%", Color = "#FF02C6DC", Target = "" });
			TaskBars.Add(new IndexTaskBar() { Icon = "PlaylistStar", Title = "备忘录", Content = "4", Color = "#FFFFA000", Target = "" });
		}
	}
}
