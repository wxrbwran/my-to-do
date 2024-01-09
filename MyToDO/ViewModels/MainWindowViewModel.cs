using MyToDO.Common;
using MyToDO.Common.Models;
using MyToDO.Extensions;
using Prism.Commands;
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
  public class MainWindowViewModel : BindableBase, IConfigureService
  {
    public MainWindowViewModel(IRegionManager regionManager)
    {
      this.regionManager = regionManager;
      MenuBars = new ObservableCollection<MenuBar>();
      // CreateMenuBars();
      NavigateCommond = new DelegateCommand<MenuBar>(Navagate);
      GoBackCommond = new DelegateCommand(() =>
      {
        if(journal != null && journal.CanGoBack)
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
    }

    private readonly IRegionManager regionManager;
    private IRegionNavigationJournal? journal;

    public DelegateCommand<MenuBar> NavigateCommond { get; private set; }
    public DelegateCommand GoBackCommond { get; private set; }
    public DelegateCommand GoForWardCommond { get; private set; }


    private ObservableCollection<MenuBar>? menuBars;
    public ObservableCollection<MenuBar> MenuBars
    {
      get{ return menuBars!; }
      set { menuBars = value; RaisePropertyChanged(); }
    }

    void CreateMenuBars()
    {
      MenuBars.Add(new MenuBar() { Icon = "Home", Title="首页", NameSpace="IndexView" });
      MenuBars.Add(new MenuBar() { Icon = "NotebookOutline", Title="待办事项", NameSpace="ToDoView" });
      MenuBars.Add(new MenuBar() { Icon = "NotebookPlus", Title="备忘录", NameSpace="MemoView" });
      MenuBars.Add(new MenuBar() { Icon = "Cog", Title="设置", NameSpace="SettingsView" });
    }

    private void Navagate(MenuBar bar)
    {
      if(bar == null || string.IsNullOrWhiteSpace(bar.NameSpace)) return;
      regionManager.Regions[PrismRegionManager.MainWindowRegionName].RequestNavigate(bar.NameSpace, back =>
      {
        journal= back.Context.NavigationService.Journal;
      });
    }
    /// <summary>
    /// 配置首页初始化参数
    /// </summary>
		public void Configure()
		{
      CreateMenuBars();
			regionManager.Regions[PrismRegionManager.MainWindowRegionName].RequestNavigate("IndexView", back =>
			{
				journal = back.Context.NavigationService.Journal;
			});
		}
	}
}
