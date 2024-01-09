using DryIoc;
using MyToDO.Common;
using MyToDO.Service;
using MyToDO.ViewModels;
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

		protected override void OnInitialized()
		{
			var configureService = App.Current.MainWindow.DataContext as IConfigureService;
      if (configureService != null)
      {
        configureService.Configure();
      }
			base.OnInitialized();
		}

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
      containerRegistry.RegisterForNavigation<IndexView, IndexViewModel>();
      containerRegistry.RegisterForNavigation<MemoView, MemoViewModel>();
      containerRegistry.RegisterForNavigation<ToDoView, ToDoViewModel>();
      containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();

      containerRegistry.RegisterForNavigation<SkinView, SkinViewModel>();
      containerRegistry.RegisterForNavigation<AboutView>();

      containerRegistry.GetContainer().Register<HttpRestClient>(
        made: Parameters.Of.Type<string>(serviceKey: "webUrl")
      );
      containerRegistry.GetContainer().RegisterInstance(@"http://localhost:57975/", serviceKey: "webUrl");

      containerRegistry.Register<IToDoService, ToDoService>();
      containerRegistry.Register<IMemoService, MemoService>();
		}
  }

}
