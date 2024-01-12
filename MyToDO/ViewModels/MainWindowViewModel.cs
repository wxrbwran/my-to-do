using MyToDO.Common;
using MyToDO.Common.Models;
using MyToDO.Extensions;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Windows;

namespace MyToDO.ViewModels
{
	public class MainWindowViewModel : BindableBase, IConfigureService
	{

		private readonly IRegionManager regionManager;
		private readonly IContainerProvider provider;
		private IRegionNavigationJournal? journal;

		public DelegateCommand<MenuBar> NavigateCommond { get; private set; }
		public DelegateCommand GoBackCommond { get; private set; }
		public DelegateCommand GoForWardCommond { get; private set; }
		public DelegateCommand LogOutCommand { get; private set; }

		public MainWindowViewModel(IRegionManager regionManager, IEventAggregator aggregator, IContainerProvider provider)
		{
			this.regionManager = regionManager;
			this.provider = provider;
			MenuBars = new ObservableCollection<MenuBar>();
			// CreateMenuBars();
			LogOutCommand = new DelegateCommand(LogOut);
			NavigateCommond = new DelegateCommand<MenuBar>(Navagate);
			GoBackCommond = new DelegateCommand(() =>
			{
				if (journal != null && journal.CanGoBack)
				{
					journal.GoBack();
				}
			});
			GoForWardCommond = new DelegateCommand(() =>
			{
				if (journal != null && journal.CanGoForward)
				{
					journal.GoForward();
				}
			});
			// 注册消息事件
			aggregator.RegisterMessage(messageModel =>
			{
				Console.WriteLine(messageModel.Message);
				SelectedMenuIndex = int.Parse(messageModel.Message);
			}, "MainWindowMenuIndex");
		}

		#region 属性
		private int selectedMenuIndex;
		public int SelectedMenuIndex
		{
			get { return selectedMenuIndex; }
			set { selectedMenuIndex = value; RaisePropertyChanged(); }
		}


		private string  username;
		public string  Username
		{
			get { return username; }
			set { username = value; RaisePropertyChanged(); }
		}


		private ObservableCollection<MenuBar>? menuBars;
		public ObservableCollection<MenuBar> MenuBars
		{
			get { return menuBars!; }
			set { menuBars = value; RaisePropertyChanged(); }
		}
		#endregion 属性

		private void LogOut()
		{
			App.LogOut(provider);
		}

		void CreateMenuBars()
		{
			MenuBars.Add(new MenuBar() { Index = 0, Icon = "Home", Title = "首页", NameSpace = "IndexView" });
			MenuBars.Add(new MenuBar() { Index = 1, Icon = "NotebookOutline", Title = "待办事项", NameSpace = "ToDoView" });
			MenuBars.Add(new MenuBar() { Index = 2, Icon = "NotebookPlus", Title = "备忘录", NameSpace = "MemoView" });
			MenuBars.Add(new MenuBar() { Index = 3, Icon = "Cog", Title = "设置", NameSpace = "SettingsView" });
		}

		private void Navagate(MenuBar bar)
		{
			if (bar == null || string.IsNullOrWhiteSpace(bar.NameSpace)) return;
			SelectedMenuIndex = bar.Index;
			regionManager.Regions[PrismRegionManager.MainWindowRegionName].RequestNavigate(bar.NameSpace, back =>
			{
				journal = back.Context.NavigationService.Journal;
			});
		}
		/// <summary>
		/// 配置首页初始化参数
		/// </summary>
		public void Configure()
		{
			CreateMenuBars();
			Username = AppSession.Username;
			regionManager.Regions[PrismRegionManager.MainWindowRegionName].RequestNavigate("IndexView", back =>
			{
				journal = back.Context.NavigationService.Journal;
			});
		}
	}
}
