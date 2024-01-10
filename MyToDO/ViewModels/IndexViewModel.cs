using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDO.Common;
using MyToDO.Common.Models;
using MyToDO.Extensions;
using MyToDO.Service;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
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
		public DelegateCommand<ToDoDto> ToDoCompleteCommand { get; private set; }
		public DelegateCommand<IndexTaskBar> NavigateCommand { get; private set; }

		private readonly IToDoService toDoService;
		private readonly IMemoService memoService;
		private readonly IEventAggregator aggregator;
		private readonly IDialogHostService dialogService;
		private readonly IRegionManager regionManager;

		public IndexViewModel(IContainerProvider provider, IEventAggregator aggregator, IDialogHostService dialogService) : base(provider)
		{
			Welcome = $"你好，小然。今天是{DateTime.Now.GetDateTimeFormats('D')[1].ToString()}。";
			TaskBars = new ObservableCollection<IndexTaskBar>();
			CreateTaskBars();

			ExecuteCommand = new DelegateCommand<string>(Execute);
			EditMemoCommand = new DelegateCommand<MemoDto>(AddEditMemo);
			EditToDoCommand = new DelegateCommand<ToDoDto>(AddEditToDo);
			ToDoCompleteCommand = new DelegateCommand<ToDoDto>(ToDoComplete);
			NavigateCommand = new DelegateCommand<IndexTaskBar>(Navigate);
			this.toDoService = provider.Resolve<IToDoService>();
			this.memoService = provider.Resolve<IMemoService>();
			this.regionManager = provider.Resolve<IRegionManager>();
			this.aggregator = aggregator;
			this.dialogService = dialogService;
		}

		#region 属性

		private string welcome;
		public string Welcome
		{
			get { return welcome; }
			set { welcome = value; RaisePropertyChanged(); }
		}


		private ObservableCollection<IndexTaskBar> taskBars;
		public ObservableCollection<IndexTaskBar> TaskBars
		{
			get { return taskBars; }
			set { taskBars = value; RaisePropertyChanged(); }
		}

		private SummaryDto summary;
		public SummaryDto Summary
		{
			get { return summary; }
			set { summary = value; RaisePropertyChanged(); }
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

		private void Navigate(IndexTaskBar bar)
		{
			if (string.IsNullOrWhiteSpace(bar.Target)) return;
			NavigationParameters param = new NavigationParameters();
			if (bar.Title == "汇总")
			{
				param.Add("Status", 0);
			}
			if (bar.Title == "已完成")
			{
				param.Add("Status", 2);
			}
			regionManager.Regions[PrismRegionManager.MainWindowRegionName]
				.RequestNavigate(bar.Target, param);
		}

		private async void ToDoComplete(ToDoDto dto)
		{
			try
			{
				ApiResponse<ToDoDto> resp = await toDoService.UpdateAsync(dto);
				if (resp.Status)
				{
					await HandleLoadData();
					aggregator.SendMessage("已完成！");
				}
			}
			catch (Exception ex) { }
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
						await HandleLoadData();
					}
				} else
				{
					// 新增
					var apiResponse = await toDoService.AddAsync(dto);
					if (apiResponse.Status)
					{
						await HandleLoadData();
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
						await HandleLoadData();
					}
				}
				else
				{
					// 新增
					var apiResponse = await memoService.AddAsync(dto);
					if (apiResponse.Status)
					{
						await HandleLoadData();
					}
				}
			}
		}

		private void CreateTaskBars()
		{
			TaskBars.Add(new IndexTaskBar() { Icon = "ClockFast", Title = "汇总", Content = "0", Color = "#FF0CA0FF", Target = "ToDoView" });
			TaskBars.Add(new IndexTaskBar() { Icon = "ClockCheckOutline", Title = "已完成", Content = "0", Color = "#FF1ECA3A", Target = "ToDoView" });
			TaskBars.Add(new IndexTaskBar() { Icon = "ChartLineVariant", Title = "完成比例", Content = "0%", Color = "#FF02C6DC", Target = "" });
			TaskBars.Add(new IndexTaskBar() { Icon = "PlaylistStar", Title = "备忘录", Content = "0", Color = "#FFFFA000", Target = "MemoView" });
		}
		private async Task HandleLoadData() 
		{
			ApiResponse<SummaryDto> apiResponse = await toDoService.SummaryAsync();
			if (apiResponse.Status)
			{
				Summary = apiResponse.Result;
				RefreshTaskBars();
			}
		}
		private void RefreshTaskBars()
		{
			taskBars[0].Content = Summary.TodoCount.ToString();
			taskBars[1].Content = Summary.CompletedToDoCount.ToString();
			taskBars[2].Content = Summary.CompletedToDoRatio;
			taskBars[3].Content = Summary.MemoCount.ToString();
		}
		public override async void OnNavigatedTo(NavigationContext navigationContext)
		{
			await HandleLoadData();
			base.OnNavigatedTo(navigationContext);
		}
	}
}
