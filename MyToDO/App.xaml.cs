﻿using MyToDO.ViewModels;
using MyToDO.Views;
using Prism.DryIoc;
using Prism.Ioc;
using System.Configuration;
using System.Data;
using System.Windows;

namespace MyToDO
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : PrismApplication
  {
    protected override Window CreateShell()
    {
      return Container.Resolve<MainWindow>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
      containerRegistry.RegisterForNavigation<IndexView, IndexViewModel>();
      containerRegistry.RegisterForNavigation<MemoView, MemoViewModel>();
      containerRegistry.RegisterForNavigation<ToDoView, ToDoViewModel>();
      containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
    }
  }

}