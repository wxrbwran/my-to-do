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

namespace MyToDO.ViewModels
{
  public class SettingsViewModel: BindableBase
  {
    private IRegionManager regionManager;

    public ObservableCollection<MenuBar> MenuBars { get; private set; }
    public DelegateCommand<MenuBar> NavigateCommond { get; private set; }

    public SettingsViewModel(IRegionManager regionManager)
    {
      this.regionManager = regionManager;
      MenuBars = new ObservableCollection<MenuBar>();
      CreateMenuBars();
      NavigateCommond = new DelegateCommand<MenuBar>(Navagate);
    }

    void CreateMenuBars()
    {
      MenuBars.Add(new MenuBar() { Icon = "Palette", Title = "个性化", NameSpace = "SkinView" });
      MenuBars.Add(new MenuBar() { Icon = "Cog", Title = "系统设置", NameSpace = "" });
      MenuBars.Add(new MenuBar() { Icon = "Information", Title = "关于更多", NameSpace = "AboutView" });
    }

    private void Navagate(MenuBar bar)
    {
      if (bar == null || string.IsNullOrWhiteSpace(bar.NameSpace)) return;
      regionManager.Regions[PrismRegionManager.SettingsViewRegionName].RequestNavigate(bar.NameSpace);
    }
  }
}
