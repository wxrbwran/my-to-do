using MyToDo.Shared.Dtos;
using MyToDO.Common;
using MyToDO.Common.Models;
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
		private readonly IDialogHostService dialogService;

		public IndexViewModel(IContainerProvider provider, IDialogHostService dialogService) : base(provider)
		{

			TaskBars = new ObservableCollection<IndexTaskBar>();
			ToDoDtos = new ObservableCollection<ToDoDto>();
			MemoDtos = new ObservableCollection<MemoDto>();
			ExecuteCommand = new DelegateCommand<string>(Execute);
			CreateTaskBars();
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
					AddToDo(); break;
				case "AddMemo":
					AddMemo(); break;
				
			}
		}

		private void AddMemo()
		{
			dialogService.ShowDialog("AddMemoView", null);

		}

		private void AddToDo()
		{
			dialogService.ShowDialog("AddToDoView", null);

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
