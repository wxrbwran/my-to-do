using DryIoc;
using MyToDO.Common;
using MyToDO.Service;
using MyToDO.ViewModels;
using MyToDO.ViewModels.Dialog;
using MyToDO.Views;
using MyToDO.Views.Dialog;
using Prism.DryIoc;
using Prism.Ioc;
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
			#region prism region
			containerRegistry.RegisterForNavigation<IndexView, IndexViewModel>();
			containerRegistry.RegisterForNavigation<MemoView, MemoViewModel>();
			containerRegistry.RegisterForNavigation<ToDoView, ToDoViewModel>();
			containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
			containerRegistry.RegisterForNavigation<SkinView, SkinViewModel>();
			containerRegistry.RegisterForNavigation<AboutView>();
			#endregion prism region

			#region 弹窗
			containerRegistry.Register<IDialogHostService, DialogHostService>();
			containerRegistry.RegisterForNavigation<AddToDoView, AddToDoViewModel>();
			containerRegistry.RegisterForNavigation<AddMemoView, AddMemoViewModel>();
			containerRegistry.RegisterForNavigation<MsgView, MsgViewModel>();
			#endregion 弹窗
			
			#region 服务
			containerRegistry.Register<IToDoService, ToDoService>();
			containerRegistry.Register<IMemoService, MemoService>();
			#endregion 服务

			#region 其他
			containerRegistry.GetContainer().Register<HttpRestClient>(
				made: Parameters.Of.Type<string>(serviceKey: "webUrl")
			);
			containerRegistry.GetContainer().RegisterInstance(@"http://localhost:57975/", serviceKey: "webUrl");
			#endregion 其他
		}
	}

}
